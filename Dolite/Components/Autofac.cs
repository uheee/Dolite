namespace Dolite.Components;

// public class AutofacComponent : DoliteComponent
// {
//     public AutofacComponent(IEnumerable<Action<ContainerBuilder>> configActions)
//     {
//         ConfigActions = configActions.ToList();
//     }
//
//     public List<Action<ContainerBuilder>> ConfigActions { get; }
//
//     public override void BeforeBuild(WebApplicationBuilder builder)
//     {
//         builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
//         builder.Host.ConfigureContainer<ContainerBuilder>((_, b) =>
//             ConfigActions.ForEach(action => action(b)));
//     }
//
//     public void AddConfig(Action<ContainerBuilder> action)
//     {
//         ConfigActions.Add(action);
//     }
// }
//
// public static class AutofacComponentExtensions
// {
//     public static DoliteBuilder UseAutofac(this DoliteBuilder builder,
//         params Action<ContainerBuilder>[] configActions)
//     {
//         var component = new AutofacComponent(configActions);
//         return builder.AddComponent(component);
//     }
//
//     // public static DoliteBuilder UseAutofac(this DoliteBuilder builder, params Module[] modules)
//     // {
//     //     var component = new AutofacComponent(b => modules.ToList().ForEach(module => b.RegisterModule(module)));
//     //     return builder.AddComponent(component);
//     // }
// }