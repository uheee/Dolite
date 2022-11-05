using Dolite.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Dolite.Components;

public class LocalizationComponent : DoliteComponent
{
    private readonly Resource _resource;

    public LocalizationComponent(Resource resource)
    {
        _resource = resource;
    }

    public override void BeforeBuild(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(_resource);
    }
}

public static class LocalizationComponentExtensions
{
    public static DoliteBuilder UseLocalization(this DoliteBuilder builder, string path)
    {
        var resource = new Resource(path);
        var component = new LocalizationComponent(resource);
        return builder.AddComponent(component);
    }
}