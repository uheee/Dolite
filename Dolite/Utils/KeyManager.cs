using System.Collections.Concurrent;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Dolite.Utils;

public class KeyManager
{
    private static string? _keyPath;
    public static string KeyPath => _keyPath ?? throw new Exception("missing key path");
    public ConcurrentDictionary<string, ECDsaSecurityKey> PrivateKeys { get; } = new();
    public ConcurrentDictionary<string, ECDsaSecurityKey> PublicKeys { get; } = new();

    public static void SetPath(string path)
    {
        _keyPath = path;
        if (!Directory.Exists(path)) Directory.CreateDirectory(_keyPath);
    }

    public ECDsaSecurityKey Private(string name)
    {
        if (PrivateKeys.TryGetValue(name, out var securityKey)) return securityKey!;
        if (!UpdateKey(name, true))
        {
            GenerateKeys(name);
            UpdateKey(name, true);
            UpdateKey(name, false);
        }

        PrivateKeys.TryGetValue(name, out securityKey);
        return securityKey!;
    }

    public ECDsaSecurityKey Public(string name)
    {
        if (PublicKeys.TryGetValue(name, out var securityKey)) return securityKey!;
        if (!UpdateKey(name, false))
        {
            GenerateKey(Private(name).ECDsa, name, false);
            UpdateKey(name, false);
        }

        PublicKeys.TryGetValue(name, out securityKey);
        return securityKey!;
    }

    private bool UpdateKey(string name, bool isPrivate)
    {
        var file = Path.Combine(KeyPath, $"{name}{(isPrivate ? "_private" : "_public")}.pem");
        if (!File.Exists(file)) return false;
        var content = File.ReadAllText(file);
        var key = ECDsa.Create();
        key.ImportFromPem(content);
        var securityKey = new ECDsaSecurityKey(key);
        (isPrivate ? PrivateKeys : PublicKeys).AddOrUpdate(name, securityKey, (_, _) => securityKey);
        return true;
    }

    private static void GenerateKeys(string name)
    {
        var ecdsa = ECDsa.Create();
        ecdsa.GenerateKey(ECCurve.NamedCurves.nistP256);
        GenerateKey(ecdsa, name, true);
        GenerateKey(ecdsa, name, false);
    }

    private static void GenerateKey(ECDsa ecdsa, string name, bool isPrivate)
    {
        var key = isPrivate
            ? ecdsa.ExportECPrivateKey()
            : ecdsa.ExportSubjectPublicKeyInfo();
        var content = new string(PemEncoding.Write(isPrivate ? "EC PRIVATE KEY" : "PUBLIC KEY", key));
        var path = Path.Combine(KeyPath, $"{name}{(isPrivate ? "_private" : "_public")}.pem");
        File.WriteAllText(path, content);
    }
}