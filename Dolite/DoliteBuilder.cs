using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Dolite;

public class DoliteBuilder
{
    public WebApplicationBuilder WebAppBuilder { get; }
    public Dictionary<Type, DoliteComponent> Components { get; } = new();

    internal DoliteBuilder(WebApplicationBuilder webAppBuilder)
    {
        WebAppBuilder = webAppBuilder;
    }

    public IConfiguration Configuration => WebAppBuilder.Configuration;

    public static DoliteBuilder Init(string[] args)
    {
        return new DoliteBuilder(WebApplication.CreateBuilder(args));
    }

    public bool HasComponent<TComponent>() where TComponent : DoliteComponent
    {
        return Components.ContainsKey(typeof(TComponent));
    }

    public TComponent? GetComponent<TComponent>() where TComponent : DoliteComponent
    {
        if (!Components.TryGetValue(typeof(TComponent), out var component))
            return null;
        return (TComponent) component;
    }

    public DoliteBuilder AddComponent(DoliteComponent component)
    {
        Components.Add(component.GetType(), component);
        return this;
    }

    public async Task Done()
    {
        Components.Values.ToList().ForEach(component => component.BeforeBuild(WebAppBuilder));
        var app = WebAppBuilder.Build();
        Components.Values.ToList().ForEach(component => component.AfterBuild(app));
        await app.RunAsync();
    }
}