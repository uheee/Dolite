using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Dolite;

public static class DoliteKey
{
    private static void GenKeys(string path)
    {
        var ecdsa = ECDsa.Create();
        ecdsa.GenerateKey(ECCurve.NamedCurves.nistP256);
        var privateKey = ecdsa.ExportECPrivateKey();
        var publicKey = ecdsa.ExportSubjectPublicKeyInfo();
        var privateContent = new string(PemEncoding.Write("EC PRIVATE KEY", privateKey));
        var publicContent = new string(PemEncoding.Write("PUBLIC KEY", publicKey));
        Directory.CreateDirectory(path);
        var privatePath = Path.Combine(path, "private.pem");
        var publicPath = Path.Combine(path, "public.pem");
        File.WriteAllText(privatePath, privateContent);
        File.WriteAllText(publicPath, publicContent);
    }

    public static SecurityKey PrivateKey(string path)
    {
        if (!Directory.Exists(path)) GenKeys(path);
        var content = File.ReadAllText(Path.Combine(path, "private.pem"));
        var key = ECDsa.Create();
        key.ImportFromPem(content);
        return new ECDsaSecurityKey(key);
    }

    public static SecurityKey PublicKey(string path)
    {
        if (!Directory.Exists(path)) GenKeys(path);
        var content = File.ReadAllText(Path.Combine(path, "public.pem"));
        var key = ECDsa.Create();
        key.ImportFromPem(content);
        return new ECDsaSecurityKey(key);
    }

    public static SecurityKey[] Keys(string path)
    {
        return new[] {PrivateKey(path), PublicKey(path)};
    }
}