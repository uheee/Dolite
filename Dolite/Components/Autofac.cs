using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Module = Autofac.Module;

namespace Dolite.Components;

public class AutofacComponent : DoliteComponent
{
    public AutofacComponent(IEnumerable<Action<ContainerBuilder>> configActions)
    {
        ConfigActions = configActions.ToList();
    }

    public List<Action<ContainerBuilder>> ConfigActions { get; }

    public override void BeforeBuild(WebApplicationBuilder builder)
    {
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        builder.Host.ConfigureContainer<ContainerBuilder>((_, b) =>
            ConfigActions.ForEach(action => action(b)));
    }
}

public static class AutofacComponentExtensions
{
    public static DoliteBuilder UseAutofac(this DoliteBuilder builder,
        params Action<ContainerBuilder>[] configActions)
    {
        var component = new AutofacComponent(configActions);
        return builder.AddComponent(component);
    }

    // public static DoliteBuilder UseAutofac(this DoliteBuilder builder, params Module[] modules)
    // {
    //     var component = new AutofacComponent(b => modules.ToList().ForEach(module => b.RegisterModule(module)));
    //     return builder.AddComponent(component);
    // }
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
    }
}