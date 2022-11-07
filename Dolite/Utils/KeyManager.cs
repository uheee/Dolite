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
    }

    public static (SecurityKey PrivateKey, SecurityKey PublicKey) Keys(string name)
    {
        if (!Directory.Exists(KeyPath)) GenKeys(name);
        var privateContent = File.ReadAllText(Path.Combine(KeyPath, $"{name}_private.pem"));
        var privateKey = ECDsa.Create();
        privateKey.ImportFromPem(privateContent);
        var publicContent = File.ReadAllText(Path.Combine(KeyPath, $"{name}_public.pem"));
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

    private static void GenKeys(string name)
    {
        var ecdsa = ECDsa.Create();
        ecdsa.GenerateKey(ECCurve.NamedCurves.nistP256);
        var privateKey = ecdsa.ExportECPrivateKey();
        var publicKey = ecdsa.ExportSubjectPublicKeyInfo();
        var privateContent = new string(PemEncoding.Write("EC PRIVATE KEY", privateKey));
        var publicContent = new string(PemEncoding.Write("PUBLIC KEY", publicKey));
        Directory.CreateDirectory(KeyPath);
        var privatePath = Path.Combine(KeyPath, $"{name}_private.pem");
        var publicPath = Path.Combine(KeyPath, $"{name}_public.pem");
        File.WriteAllText(privatePath, privateContent);
        File.WriteAllText(publicPath, publicContent);
    }
}