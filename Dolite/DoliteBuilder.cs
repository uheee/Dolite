using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Dolite.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Module = Autofac.Module;

namespace Dolite;

public class DoliteBuilder
{
    internal DoliteBuilder(WebApplicationBuilder webAppBuilder)
    {
        WebAppBuilder = webAppBuilder;
    }

    public WebApplicationBuilder WebAppBuilder { get; }
    public Dictionary<Type, DoliteComponent> Components { get; } = new();

    internal List<Action<ContainerBuilder>> AutofacConfigActions { get; } = new();

    public static DoliteBuilder Init(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        return new DoliteBuilder(builder);
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
        WebAppBuilder.Host.ConfigureContainer<ContainerBuilder>((_, b) =>
            AutofacConfigActions.ForEach(action => action(b)));
        var app = WebAppBuilder.Build();
        Components.Values.ToList().ForEach(component => component.AfterBuild(app));
        await app.RunAsync();
    }
}

public static class DoliteBuilderExtensions
{
    public static DoliteBuilder Inject(this DoliteBuilder builder, Action<ContainerBuilder> autofacConfigAction)
    {
        builder.AutofacConfigActions.Add(autofacConfigAction);
        return builder;
    }

    public static DoliteBuilder Awesome(this DoliteBuilder builder)
    {
        return builder.Inject(b => b.RegisterModule(new AwesomeModule(Assembly.GetExecutingAssembly())));
    }
}

public class AwesomeModule : Module
{
    private readonly Assembly _assembly;

    public AwesomeModule(Assembly assembly)
    {
        _assembly = assembly;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(_assembly)
            .Where(type => type.IsAssignableTo<ControllerBase>())
            .PropertiesAutowired();
        builder.RegisterAssemblyTypes(_assembly)
            .Where(type => type.Name.EndsWith("Impl"))
            .AsImplementedInterfaces()
            .PropertiesAutowired();
        builder.RegisterType<ExceptionFactory>()
            .AsSelf()
            .PropertiesAutowired();
    }
}