using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Dolite.Components;

public class ControllerComponent : DoliteComponent
{
    public override void BeforeBuild(WebApplicationBuilder builder)
    {
        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        builder.Services.AddControllers();
        builder.Services.AddMvcCore().AddApiExplorer().AddControllersAsServices().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });
        builder.Services.AddHttpContextAccessor();
    }

    public override void AfterBuild(WebApplication app)
    {
        app.MapControllers();
    }
}

public static class ControllerComponentExtensions
{
    public static DoliteBuilder UseControllers(this DoliteBuilder builder)
    {
        var component = new ControllerComponent();
        return builder.AddComponent(component);
    }
}