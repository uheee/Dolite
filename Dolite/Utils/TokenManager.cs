using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Dolite.Utils;

public static class TokenManager
{
    public static string Generate(string keyName, JwtPayload payload)
    {
        var privateKey = KeyManager.Private(keyName);
        var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.EcdsaSha256);
        var descriptor = new JwtSecurityToken(new JwtHeader(credentials), payload);

        return new JwtSecurityTokenHandler().WriteToken(descriptor);
    }
    
    public static string Generate(string keyName, TimeSpan expiration, params Claim[] claims)
    {
        var privateKey = KeyManager.Private(keyName);
        var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.EcdsaSha256);
        var currentTime = DateTime.Now;
        var expiresTime = currentTime + expiration;
        var descriptor = new JwtSecurityToken(signingCredentials: credentials, claims: claims,
            expires: expiresTime, notBefore: currentTime);

        return new JwtSecurityTokenHandler().WriteToken(descriptor);
    }
}