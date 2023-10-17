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

internal sealed class UsersService : IUsersService
{
    private readonly IApplicationTokenService _applicationTokenService;
    private readonly AppCredentialsConfiguration _appCredentialsConfiguration;

    public UsersService(
        IApplicationTokenService applicationTokenService,
        IOptions<AppCredentialsConfiguration> appCredentialsConfiguration)
    {
        _applicationTokenService = applicationTokenService;
        _appCredentialsConfiguration = appCredentialsConfiguration.Value;
    }

    public async Task<ErrorOr<UserInfoResponse>> CreateUserAsync(
        CreateUserRequest request, CancellationToken cancellationToken)
    {
        var baseUrl = $"https://{_appCredentialsConfiguration.ClientInfo.Domain}/";
        var restClient = RestService.For<IUsersApi>(baseUrl, RefitSettingsExtension.ProjectDefaultSettings);

        return await DoApiRequestInternalAsync(
            request,
            Auth0CreateUserRequest.FromCreateUserRequest,
            restClient.CreateUserAsync,
            cancellationToken);
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

    public async Task<ErrorOr<UserInfoResponse>> ChangePasswordAsync(
        ChangeUserPasswordRequest request, CancellationToken cancellationToken)
    {
        var baseUrl = $"https://{_appCredentialsConfiguration.ClientInfo.Domain}/";
        var restClient = RestService.For<IUsersApi>(baseUrl, RefitSettingsExtension.ProjectDefaultSettings);

        var userToken = await restClient.ChangePasswordAsync(
            request.UserId!,
            Auth0ChangeUserPasswordRequest.FromChangeUserPasswordRequest(request),
            await GetDefaultHeadersAsync(),
            cancellationToken);

        if (!userToken.IsSuccessStatusCode)
        {
            return Error.Unexpected("Users.ChangePassword", userToken.Error.Content!);
        }

        return userToken.Content;
    }

    private async Task<ErrorOr<TResponse>> DoApiRequestInternalAsync<TModel, TRequest, TResponse>(
        TModel internalRequest,
        Func<TModel, TRequest> requestMapper,
        Func<TRequest, IDictionary<string, string>, CancellationToken, Task<ApiResponse<TResponse>>> apiDelegate,
        CancellationToken ct)
    {
        var request = requestMapper(internalRequest);

        var response = await apiDelegate(
            request, await GetDefaultHeadersAsync(), ct);

        if (!response.IsSuccessStatusCode)
        {
            return Error.Unexpected(apiDelegate.Method.Name, response.Error.Content!);
        }

        return response.Content;
    }

    private async ValueTask<IDictionary<string, string>> GetDefaultHeadersAsync()
    {
        return new Dictionary<string, string>
        {
            {
                "Authorization",
                $"Bearer {(await _applicationTokenService.GetApplicationCredentialsAsync())!.AccessToken}"
            }
        };
    }
}