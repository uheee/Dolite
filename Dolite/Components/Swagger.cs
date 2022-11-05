using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Dolite.Components;

public class SwaggerComponent : DoliteComponent
{
    private readonly Assembly _assembly;
    private readonly bool _auth;
    private readonly OpenApiInfo? _openApiInfo;

    public SwaggerComponent(Assembly assembly, OpenApiInfo? openApiInfo, bool auth)
    {
        _openApiInfo = openApiInfo;
        _assembly = assembly;
        _auth = auth;
    }

    public override void BeforeBuild(WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            if (_openApiInfo is not null) options.SwaggerDoc(_openApiInfo.Version, _openApiInfo);
            var xmlFilename = $"{_assembly.GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            if (_auth)
            {
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. Example: 'Authorization: Bearer {token}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            }
        });
    }

    public override void AfterBuild(WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return;
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}

public static class SwaggerComponentExtensions
{
    public static DoliteBuilder UseSwagger(this DoliteBuilder builder, Assembly assembly, OpenApiInfo? openApiInfo = null)
    {
        var component = new SwaggerComponent(assembly, openApiInfo, builder.HasComponent<AuthComponent>());
        return builder.AddComponent(component);
    }
}