using Microsoft.AspNetCore.Builder;
using Serilog;

namespace Dolite.Components;

public class SerilogComponent : DoliteComponent
{
    public override void BeforeBuild(WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));
    }
}

public static class SerilogComponentExtensions
{
    public static DoliteBuilder UseSerilog(this DoliteBuilder builder)
    {
        var component = new SerilogComponent();
        return builder.AddComponent(component);
    }
}