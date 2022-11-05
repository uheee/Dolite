using Autofac;
using Dolite.Utils;

namespace Dolite.Components;

public static class LocalizationComponentExtensions
{
    public static DoliteBuilder UseLocalization(this DoliteBuilder builder, string path)
    {
        var autofacComponent = builder.GetComponent<AutofacComponent>();
        if (autofacComponent is null) throw new Exception("missing Autofac component");
        var resource = new Resource(path);
        autofacComponent.AddConfig(b => b.RegisterInstance(resource).AsSelf());
        return builder;
    }
}