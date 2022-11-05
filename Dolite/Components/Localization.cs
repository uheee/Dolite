using Autofac;
using Dolite.Utils;

namespace Dolite.Components;

public static class LocalizationComponentExtensions
{
    public static DoliteBuilder UseLocalization(this DoliteBuilder builder, string path)
    {
        var autofacComponent = builder.GetComponent<AutofacComponent>();
        var resource = new Resource(path);
        if (autofacComponent is null) throw new Exception("missing Autofac component");
        autofacComponent.AddConfig(b => b.RegisterInstance(resource).AsSelf());
        return builder;
    }
}