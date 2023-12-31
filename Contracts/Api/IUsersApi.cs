﻿using Refit;
using SecurityLabs.Contracts.Api.External;
using SecurityLabs.Contracts.Api.Models;

namespace SecurityLabs.Contracts.Api;

internal interface IUsersApi
{
    [Post("/api/v2/users")]
    Task<ApiResponse<UserInfoResponse>> CreateUserAsync(Auth0CreateUserRequest request,
        [HeaderCollection] IDictionary<string, string> headers,
        CancellationToken cancellationToken);

    [Patch("/api/v2/users/{userId}")]
    Task<ApiResponse<UserInfoResponse>> ChangePasswordAsync(
        [AliasAs("userId")] string userId,
        Auth0ChangeUserPasswordRequest request,
        [HeaderCollection] IDictionary<string, string> headers,
        CancellationToken cancellationToken);

    [Post("/oauth/token")]
    Task<ApiResponse<AuthInfoWithRefreshTokenResponse>> GetAuthInfoFromCodeAsync(GetAuthInfoFromCodeRequest request,
        [HeaderCollection] IDictionary<string, string> headers,
        CancellationToken cancellationToken);

    [Post("/oauth/token")]
    Task<ApiResponse<AuthInfoWithRefreshTokenResponse>> GetUserTokenAsync(GetUserTokenRequest request,
        [HeaderCollection] IDictionary<string, string> headers,
        CancellationToken cancellationToken);

    [Post("/oauth/token")]
    Task<ApiResponse<AuthInfoWithRefreshTokenResponse>> RefreshAccessTokenAsync(Auth0RefreshAccessTokenRequest request,
        [HeaderCollection] IDictionary<string, string> dictionary,
        CancellationToken cancellationToken);
}