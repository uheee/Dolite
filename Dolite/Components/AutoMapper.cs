using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Dolite.Components;

public class AutoMapperComponent : DoliteComponent
{
    public AutoMapperComponent(IEnumerable<Action<IMapperConfigurationExpression>> configActions)
    {
        ConfigActions = configActions.ToList();
    }

    public List<Action<IMapperConfigurationExpression>> ConfigActions { get; }

    public override void BeforeBuild(WebApplicationBuilder builder)
    {
        ConfigActions.ForEach(action => builder.Services.AddAutoMapper(action));
    }
}

public static class AutoMapperComponentExtensions
{
    public static DoliteBuilder UseAutoMapper(this DoliteBuilder builder,
        params Action<IMapperConfigurationExpression>[] configActions)
    {
        var component = new AutoMapperComponent(configActions);
        return builder.AddComponent(component);
    }
}