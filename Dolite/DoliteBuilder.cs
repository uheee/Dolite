using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Dolite;

public class DoliteBuilder
{
    private readonly WebApplicationBuilder _builder;
    private readonly Dictionary<Type, DoliteComponent> _components = new();

    internal DoliteBuilder(WebApplicationBuilder builder)
    {
        _builder = builder;
    }

    public IConfiguration Configuration => _builder.Configuration;

    public static DoliteBuilder Init(string[] args)
    {
        return new DoliteBuilder(WebApplication.CreateBuilder(args));
    }

    public bool HasComponent<TComponent>() where TComponent : DoliteComponent
    {
        return _components.ContainsKey(typeof(TComponent));
    }

    public TComponent? GetComponent<TComponent>() where TComponent : DoliteComponent
    {
        if (!_components.TryGetValue(typeof(TComponent), out var component))
            return null;
        return (TComponent) component;
    }

    public DoliteBuilder AddComponent(DoliteComponent component)
    {
        _components.Add(component.GetType(), component);
        return this;
    }

    public async Task Done()
    {
        _components.Values.ToList().ForEach(component => component.BeforeBuild(_builder));
        var app = _builder.Build();
        _components.Values.ToList().ForEach(component => component.AfterBuild(app));
        await app.RunAsync();
    }
}