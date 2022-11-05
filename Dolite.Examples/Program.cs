using System.Reflection;
using Autofac;
using Dolite;
using Dolite.Components;

await DoliteBuilder.Init(args)
    .UseSerilog()
    .UseAutofac(builder => builder.RegisterModule(new AwesomeModule(Assembly.GetExecutingAssembly())))
    .UseAutoMapper(Assembly.GetExecutingAssembly())
    .UseAuth(DoliteKey.Keys("key"))
    .UseSwagger(Assembly.GetExecutingAssembly())
    .UseErrorHandler()
    .UseControllers()
    .Done();