using ErrorOr;
using Microsoft.Extensions.Options;
using Refit;
using SecurityLabs.Configurations;
using SecurityLabs.Contracts.Api;
using SecurityLabs.Contracts.Api.External;
using SecurityLabs.Contracts.Api.Models;
using SecurityLabs.Extensions;
using SecurityLabs.Services.Interfaces;

namespace SecurityLabs.Services;

internal class AccessTokenService : BaseRestService, IAccessTokenService
{
    private readonly AppCredentialsConfiguration _appCredentialsConfiguration;

    public AccessTokenService(
        IApplicationTokenService applicationTokenService,
        IOptions<AppCredentialsConfiguration> appCredentialsConfiguration) : base(applicationTokenService)
    {
        ArgumentNullException.ThrowIfNull(appCredentialsConfiguration.Value);
        _appCredentialsConfiguration = appCredentialsConfiguration.Value;
    }

    public async Task<ErrorOr<AuthInfoWithRefreshTokenResponse>> GetAuthInfoFromCodeAsync(
        GetAuthInfoFromCodeRequest request, CancellationToken cancellationToken)
    {
        var baseUrl = $"https://{_appCredentialsConfiguration.ClientInfo.Domain}/";
        var restClient = RestService.For<IUsersApi>(baseUrl, RefitSettingsExtension.ProjectDefaultSettings);

        return await
            DoApiRequestInternalAsync<
                GetAuthInfoFromCodeRequest, GetAuthInfoFromCodeRequest, AuthInfoWithRefreshTokenResponse>(
                    request,
                    r =>
                    {
                        SetClientCredentials(r);
                        return r;
                    },
                    restClient.GetAuthInfoFromCodeAsync,
                    cancellationToken);

        void SetClientCredentials(GetAuthInfoFromCodeRequest req)
        {
            req.ClientId = _appCredentialsConfiguration.ClientInfo.ClientId;
            req.ClientSecret = _appCredentialsConfiguration.ClientInfo.ClientSecret;
            req.Audience = _appCredentialsConfiguration.ClientInfo.Audience;
        }
    }

    public async Task<ErrorOr<AuthInfoWithRefreshTokenResponse>> GetUserTokenAsync(
        GetUserTokenRequest request, CancellationToken cancellationToken)
    {
        var baseUrl = $"https://{_appCredentialsConfiguration.ClientInfo.Domain}/";
        var restClient = RestService.For<IUsersApi>(baseUrl, RefitSettingsExtension.ProjectDefaultSettings);

        return await
            DoApiRequestInternalAsync<GetUserTokenRequest, GetUserTokenRequest, AuthInfoWithRefreshTokenResponse>(
                request,
                r =>
                {
                    SetClientCredentials(r);
                    return r;
                },
                restClient.GetUserTokenAsync,
                cancellationToken);

        void SetClientCredentials(GetUserTokenRequest req)
        {
            req.ClientId = _appCredentialsConfiguration.ClientInfo.ClientId;
            req.ClientSecret = _appCredentialsConfiguration.ClientInfo.ClientSecret;
            req.Audience = _appCredentialsConfiguration.ClientInfo.Audience;
        }
    }

    public async Task<ErrorOr<AuthInfoWithRefreshTokenResponse>> RefreshUserTokenAsync(
        RefreshAccessTokenRequest request, CancellationToken cancellationToken)
    {
        var baseUrl = $"https://{_appCredentialsConfiguration.ClientInfo.Domain}/";
        var restClient = RestService.For<IUsersApi>(baseUrl, RefitSettingsExtension.ProjectDefaultSettings);

        return await DoApiRequestInternalAsync(
            request,
            r => Auth0RefreshAccessTokenRequest.FromRefreshTokenRequest(
                r, _appCredentialsConfiguration.ClientInfo.ClientId,
                _appCredentialsConfiguration.ClientInfo.ClientSecret),
            restClient.RefreshAccessTokenAsync,
            cancellationToken);
    }
}