using System.Reflection;
using Dolite;
using Dolite.Components;
using Dolite.Utils;

KeyManager.SetPath("keys");
await DoliteBuilder.Init(args)
    .Awesome()
    .UseSerilog()
    .UseAutoMapper(Assembly.GetExecutingAssembly())
    .UseLocalization("localization")
    .UseFreeSql(Assembly.GetExecutingAssembly(), "Default", true)
    .UseAuth("user")
    .UseSwagger(Assembly.GetExecutingAssembly())
    .UseErrorHandler()
    .UseControllers()
    .Done();