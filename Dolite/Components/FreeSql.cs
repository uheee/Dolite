using System.Reflection;
using FreeSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dolite.Components;

public class FreeSqlComponent : DoliteComponent
{
    private readonly Assembly _assembly;
    private readonly bool _autoSync;
    private readonly Action<IFreeSql>? _configAction;
    private readonly string _connectionString;

    public FreeSqlComponent(Assembly assembly, string connectionString, bool autoSync, Action<IFreeSql>? configAction)
    {
        _assembly = assembly;
        _connectionString = connectionString;
        _autoSync = autoSync;
        _configAction = configAction;
    }

    public override void BeforeBuild(WebApplicationBuilder builder)
    {
        var freeSql = new FreeSqlBuilder()
            .UseConnectionString(DataType.PostgreSQL, _connectionString)
            .UseAutoSyncStructure(_autoSync)
            .Build();
        _configAction?.Invoke(freeSql);
        builder.Services.AddSingleton(freeSql);
        builder.Services.AddFreeRepository(null, _assembly);
    }
}

public static class FreeSqlComponentExtensions
{
    public static DoliteBuilder UseFreeSql(this DoliteBuilder builder, Assembly assembly,
        string connectionName = "Default", bool autoSync = false, Action<IFreeSql>? configAction = null)
    {
        var connectionString = builder.Configuration.GetConnectionString(connectionName);
        var component = new FreeSqlComponent(assembly, connectionString, autoSync, configAction);
        return builder.AddComponent(component);
    }
}