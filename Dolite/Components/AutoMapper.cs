using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Dolite.Components;

public class AutoMapperComponent : DoliteComponent
{
    public AutoMapperComponent(IEnumerable<Assembly> assemblies)
    {
        Assemblies = assemblies.ToList();
    }

    public List<Assembly> Assemblies { get; }

    public override void BeforeBuild(WebApplicationBuilder builder)
    {
        Assemblies.ForEach(assembly => builder.Services.AddAutoMapper(assembly));
    }
}

public static class AutoMapperComponentExtensions
{
    public static DoliteBuilder UseAutoMapper(this DoliteBuilder builder,
        params Assembly[] assemblies)
    {
        var component = new AutoMapperComponent(assemblies);
        return builder.AddComponent(component);
    }
}