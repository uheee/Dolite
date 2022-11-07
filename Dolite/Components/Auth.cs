using Dolite.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Dolite.Components;

public class AuthComponent : DoliteComponent
{
    private readonly Action<AuthorizationOptions>? _authorizationConfig;
    private readonly SecurityKey _publicKey;

    public AuthComponent(SecurityKey publicKey, Action<AuthorizationOptions>? authorizationConfig)
    {
        _publicKey = publicKey;
        _authorizationConfig = authorizationConfig;
    }

    public override void BeforeBuild(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidTypes = new[] {"JWT"},
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _publicKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    RequireExpirationTime = true
                };
            });
        if (_authorizationConfig is not null) builder.Services.AddAuthorization(_authorizationConfig);
    }

    public override void AfterBuild(WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}

public static class AuthComponentExtensions
{
    public static DoliteBuilder UseAuth(this DoliteBuilder builder, string keyName, Action<AuthorizationOptions>? authorizationConfig = null)
    {
        var component = new AuthComponent(KeyManager.Public(keyName), authorizationConfig);
        return builder.AddComponent(component);
    }
}