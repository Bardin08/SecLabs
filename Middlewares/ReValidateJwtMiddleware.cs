using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using SecurityLabs.Configurations;
using SecurityLabs.Contracts.Api.Models;
using SecurityLabs.Services.Proxies.Interfaces;

namespace SecurityLabs.Middlewares;

internal class ReValidateJwtMiddleware
{
    private readonly IAccessTokenServiceProxy _accessTokenServiceProxy;
    private readonly IMemoryCache _memoryCache;
    private readonly JwtTokenConfiguration _jwtTokenOptions;
    private readonly RequestDelegate _next;

    public ReValidateJwtMiddleware(
        IAccessTokenServiceProxy accessTokenServiceProxy,
        IMemoryCache memoryCache,
        IOptions<JwtTokenConfiguration> jwtTokenOptions,
        RequestDelegate next)
    {
        _accessTokenServiceProxy = accessTokenServiceProxy;
        _memoryCache = memoryCache;
        _jwtTokenOptions = jwtTokenOptions.Value;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        const string authHeaderName = "Authorization";
        const int tokenLifetimeThreshold = 5;

        if (_jwtTokenOptions.SkipRoutes.Any(
                r => r.Equals(context.Request.Path, StringComparison.InvariantCultureIgnoreCase)))
        {
            await _next(context);
            return;
        }

        await SetNewAccessTokenInternalAsync(context, authHeaderName, tokenLifetimeThreshold);
        await _next(context);
    }

    private async Task SetNewAccessTokenInternalAsync(HttpContext context, string authHeaderName,
        int tokenLifetimeThreshold)
    {
        if (context.Request.Headers.TryGetValue(authHeaderName, out var header))
        {
            var tokenStr = header.ToString()
                .Split(' ')
                .LastOrDefault();

            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(tokenStr))
            {
                var token = handler.ReadJwtToken(tokenStr);
                var remainingLifetime = token.ValidTo - DateTime.UtcNow;

                if (remainingLifetime.TotalMinutes <= tokenLifetimeThreshold)
                {
                    var refreshedAccessToken = await GetRefreshedAccessTokenAsync(token.Subject);

                    if (!string.IsNullOrEmpty(refreshedAccessToken))
                    {
                        context.Response.Headers.Authorization =
                            new StringValues($"Bearer {refreshedAccessToken}");
                    }
                }
            }
        }
    }

    private async Task<string> GetRefreshedAccessTokenAsync(string userId)
    {
        var refreshToken = _memoryCache.Get<string>(userId);
        if (string.IsNullOrEmpty(refreshToken))
        {
            return string.Empty;
        }

        var getTokenResponse = await _accessTokenServiceProxy.RefreshUserTokenAsync(
            new RefreshAccessTokenRequest
            {
                RefreshToken = refreshToken,
                Scope = "offline_access"
            }, CancellationToken.None);

        return getTokenResponse.IsError
            ? string.Empty
            : getTokenResponse.Value.AccessToken;
    }
}