using System.Reflection;
using Autofac;
using Dolite;
using Dolite.Components;
using Dolite.Utils;

KeyManager.SetPath("keys");
await DoliteBuilder.Init(args)
    .UseSerilog()
    .UseAutofac(builder => builder.RegisterModule(new AwesomeModule(Assembly.GetExecutingAssembly())))
    .UseAutoMapper(Assembly.GetExecutingAssembly())
    .UseLocalization("localization")
    .UseAuth("user")
    .UseSwagger(Assembly.GetExecutingAssembly())
    .UseErrorHandler()
    .UseControllers()
    .Done();