using Microsoft.Extensions.Configuration;

namespace Dolite.Utils;

public class Resource
{
    private readonly Dictionary<string, IConfiguration> _configurations;

    public Resource(string path)
    {
        var dir = new DirectoryInfo(path);
        _configurations = dir.EnumerateFiles("*.json").Select(fileInfo =>
        {
            var config = new ConfigurationBuilder().AddJsonFile(fileInfo.FullName).Build() as IConfiguration;
            var culture = Path.GetFileNameWithoutExtension(fileInfo.Name);
            return new {culture, config};
        }).ToDictionary(o => o.culture, o => o.config);
    }

    public IConfiguration this[string culture] => _configurations[culture];
}