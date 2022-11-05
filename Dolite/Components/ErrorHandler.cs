using System.Net.Mime;
using Dolite.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Dolite.Components;

public class ErrorHandlerComponent : DoliteComponent
{
    public override void AfterBuild(WebApplication app)
    {
        app.UseExceptionHandler(appBuilder =>
        {
            appBuilder.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerPathFeature>()!.Error;
                var error = ExceptionToError(exception, app.Environment.IsDevelopment());
                context.Response.ContentType = MediaTypeNames.Application.Json;
                context.Response.StatusCode = exception switch
                {
                    BusinessException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                };
                var jsonOptions = app.Services.GetService<IOptions<JsonOptions>>()!.Value;
                await context.Response.WriteAsJsonAsync(error, jsonOptions.JsonSerializerOptions);
            });
        });
    }

    private ErrorInfo? ExceptionToError(Exception? exception, bool isDevelopment)
    {
        return exception switch
        {
            null => null,
            BusinessException businessException => new ErrorInfo
            {
                ErrCode = businessException.ErrCode,
                ErrMsg = businessException.ErrMsg
            },
            _ => new ErrorInfo
            {
                ErrMsg = exception.Message,
                Stacktrace = exception.StackTrace?.Split(Environment.NewLine).Select(s => s.Trim()),
                Inner = isDevelopment
                    ? ExceptionToError(exception.InnerException, isDevelopment)
                    : null
            }
        };
    }
}

public static class ErrorHandlerComponentExtensions
{
    public static DoliteBuilder UseErrorHandler(this DoliteBuilder builder)
    {
        var component = new ErrorHandlerComponent();
        return builder.AddComponent(component);
    }
}

public class BusinessException : Exception
{
    public BusinessException(int errCode, string errMsg) : base($"{errCode}: {errMsg}")
    {
        ErrCode = errCode;
        ErrMsg = errMsg;
    }

    public int ErrCode { get; }
    public string ErrMsg { get; }
}