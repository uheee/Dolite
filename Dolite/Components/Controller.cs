using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Dolite.Components;

public class ControllerComponent : DoliteComponent
{
    public override void BeforeBuild(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddMvcCore().AddApiExplorer().AddControllersAsServices()
            .AddNewtonsoftJson(options => options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore);
        builder.Services.AddRouting(options => options.LowercaseUrls = true);
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