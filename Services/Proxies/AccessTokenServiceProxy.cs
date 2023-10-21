using System.IdentityModel.Tokens.Jwt;
using ErrorOr;
using Microsoft.Extensions.Caching.Memory;
using SecurityLabs.Contracts.Api.Models;
using SecurityLabs.Services.Interfaces;
using SecurityLabs.Services.Proxies.Interfaces;

namespace SecurityLabs.Services.Proxies;

internal class AccessTokenServiceProxy : IAccessTokenServiceProxy
{
    private readonly IAccessTokenService _accessTokenService;
    private readonly IMemoryCache _memoryCache;

    public AccessTokenServiceProxy(
        IAccessTokenService accessTokenService, IMemoryCache memoryCache)
    {
        _accessTokenService = accessTokenService;
        _memoryCache = memoryCache;
    }

    public async Task<ErrorOr<AuthInfoWithRefreshTokenResponse>> GetAuthInfoFromCodeAsync(
        GetAuthInfoFromCodeRequest request, CancellationToken cancellationToken)
    {
        var accessTokenResponse = await _accessTokenService
            .GetAuthInfoFromCodeAsync(request, cancellationToken);

        if (accessTokenResponse.IsError)
        {
            return accessTokenResponse;
        }

        TryCacheRefreshToken(accessTokenResponse.Value);

        return accessTokenResponse;
    }

    public async Task<ErrorOr<AuthInfoWithRefreshTokenResponse>> GetUserTokenAsync(
        GetUserTokenRequest request, CancellationToken cancellationToken)
    {
        var accessTokenResponse = await _accessTokenService
            .GetUserTokenAsync(request, cancellationToken);

        if (accessTokenResponse.IsError)
        {
            return accessTokenResponse;
        }

        TryCacheRefreshToken(accessTokenResponse.Value);

        return accessTokenResponse;
    }

    public async Task<ErrorOr<AuthInfoWithRefreshTokenResponse>> RefreshUserTokenAsync(
        RefreshAccessTokenRequest request,
        CancellationToken cancellationToken)
    {
        var accessTokenResponse = await _accessTokenService
            .RefreshUserTokenAsync(request, cancellationToken);

        if (accessTokenResponse.IsError)
        {
            return accessTokenResponse;
        }

        TryCacheRefreshToken(accessTokenResponse.Value);

        return accessTokenResponse;
    }

    private void TryCacheRefreshToken(AuthInfoWithRefreshTokenResponse authInfo)
    {
        try
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            if (!jwtHandler.CanReadToken(authInfo.AccessToken))
            {
                return;
            }

            var accessToken = jwtHandler.ReadJwtToken(authInfo.AccessToken);
            _memoryCache.Set(accessToken.Subject, authInfo.RefreshToken);
        }
        catch
        {
            // The cache added here is used at the ReValidateJwtMiddleware.

            // In case of error just skip. No matter as it's a cache and maybe on the next request here won't be an error.
            // Else after access token expiration user gonna be logged out, and have to complete a sign-on again.
        }
    }
}