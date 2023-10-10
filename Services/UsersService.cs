using ErrorOr;
using Microsoft.Extensions.Options;
using Refit;
using SecurityLabs.Configurations;
using SecurityLabs.Contracts.Api;
using SecurityLabs.Contracts.Api.External;
using SecurityLabs.Contracts.Api.Models;
using SecurityLabs.Endpoints.Users;
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

        var createUserRequest = Auth0CreateUserRequest.FromCreateUserRequest(
            request,
            _appCredentialsConfiguration.ClientInfo.ClientId,
            _appCredentialsConfiguration.ClientInfo.ClientSecret,
            _appCredentialsConfiguration.ClientInfo.Audience);

        var userInfo = await restClient.CreateUserAsync(
            createUserRequest,
            new Dictionary<string, string>
            {
                {
                    "Authorization", (await _applicationTokenService.GetApplicationCredentialsAsync())!.AccessToken
                }
            },
            cancellationToken);

        if (!userInfo.IsSuccessStatusCode)
        {
            return Error.Unexpected("Users.CreateUser", userInfo.Error.Content!);
        }

        return userInfo.Content;
    }

    public async Task<ErrorOr<AuthInfoWithRefreshTokenResponse>> GetUserTokenAsync(
        GetUserTokenRequest request, CancellationToken cancellationToken)
    {
        var baseUrl = $"https://{_appCredentialsConfiguration.ClientInfo.Domain}/";
        var restClient = RestService.For<IUsersApi>(baseUrl, RefitSettingsExtension.ProjectDefaultSettings);

        request.ClientId = _appCredentialsConfiguration.ClientInfo.ClientId;
        request.ClientSecret = _appCredentialsConfiguration.ClientInfo.ClientSecret;
        request.Audience = _appCredentialsConfiguration.ClientInfo.Audience;

        var userToken = await restClient.GetUserTokenAsync(
            request,
            new Dictionary<string, string>
            {
                { "Authorization", (await _applicationTokenService.GetApplicationCredentialsAsync())!.AccessToken }
            },
            cancellationToken);

        if (!userToken.IsSuccessStatusCode)
        {
            return Error.Unexpected("Users.GetUserToken", userToken.Error.Content!);
        }

        return userToken.Content;
    }

    public async Task<ErrorOr<AuthInfoWithRefreshTokenResponse>> RefreshUserTokenAsync(
        RefreshAccessTokenRequest request, CancellationToken cancellationToken)
    {
        var baseUrl = $"https://{_appCredentialsConfiguration.ClientInfo.Domain}/";
        var restClient = RestService.For<IUsersApi>(baseUrl, RefitSettingsExtension.ProjectDefaultSettings);

        var refreshTokenRequest = Auth0RefreshAccessTokenRequest.FromRefreshTokenRequest(
            request,
            _appCredentialsConfiguration.ClientInfo.ClientId,
            _appCredentialsConfiguration.ClientInfo.ClientSecret);

        var userToken = await restClient.RefreshAccessTokenAsync(
            refreshTokenRequest,
            new Dictionary<string, string>
            {
                { "Authorization", (await _applicationTokenService.GetApplicationCredentialsAsync())!.AccessToken }
            },
            cancellationToken);

        if (!userToken.IsSuccessStatusCode)
        {
            return Error.Unexpected("Users.GetUserToken", userToken.Error.Content!);
        }

        return userToken.Content;
    }

    public async Task<ErrorOr<UserInfoResponse>> ChangePasswordAsync(
        ChangeUserPasswordRequest request, CancellationToken cancellationToken)
    {
        var baseUrl = $"https://{_appCredentialsConfiguration.ClientInfo.Domain}/";
        var restClient = RestService.For<IUsersApi>(baseUrl, RefitSettingsExtension.ProjectDefaultSettings);

        var applicationToken = (await _applicationTokenService.GetApplicationCredentialsAsync())!.AccessToken;
        var userToken = await restClient.ChangePasswordAsync(
            request.UserId!,
            Auth0ChangeUserPasswordRequest.FromChangeUserPasswordRequest(request), 
            new Dictionary<string, string>
            {
                { "Authorization", $"Bearer {applicationToken}" }
            },
            cancellationToken);

        if (!userToken.IsSuccessStatusCode)
        {
            return Error.Unexpected("Users.ChangePassword", userToken.Error.Content!);
        }

        return userToken.Content;
    }
}