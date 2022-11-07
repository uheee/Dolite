using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Dolite.Utils;

public static class KeyManager
{
    private static string? _keyPath;

    public static string KeyPath => _keyPath ?? throw new Exception("missing key path");

    public static void SetPath(string path)
    {
        _keyPath = path;
        if (!Directory.Exists(path)) Directory.CreateDirectory(_keyPath);
    }

    public static (SecurityKey PrivateKey, SecurityKey PublicKey) Keys(string name)
    {
        var privateFile = Path.Combine(KeyPath, $"{name}_private.pem");
        if (!File.Exists(privateFile)) GenerateKeys(name);
        var privateContent = File.ReadAllText(privateFile);
        var privateKey = ECDsa.Create();
        privateKey.ImportFromPem(privateContent);
        var publicFile = Path.Combine(KeyPath, $"{name}_public.pem");
        if (!File.Exists(publicFile)) GenerateKey(privateKey, name, false);
        var publicContent = File.ReadAllText(publicFile);
        var publicKey = ECDsa.Create();
        publicKey.ImportFromPem(publicContent);
        return (new ECDsaSecurityKey(privateKey), new ECDsaSecurityKey(publicKey));
    }

    public static SecurityKey Private(string name)
    {
        return Keys(name).PrivateKey;
    }

    public static SecurityKey Public(string name)
    {
        return Keys(name).PublicKey;
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