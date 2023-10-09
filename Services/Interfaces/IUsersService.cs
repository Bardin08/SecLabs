using ErrorOr;
using SecurityLabs.Contracts.Api.Models;
using SecurityLabs.Endpoints.Users;

namespace SecurityLabs.Services.Interfaces;

internal interface IUsersService
{
    Task<ErrorOr<UserInfoResponse>> CreateUserAsync(
        CreateUserRequest request, CancellationToken cancellationToken);

    Task<ErrorOr<AuthInfoWithRefreshTokenResponse>> GetUserTokenAsync(
        GetUserTokenRequest request, CancellationToken cancellationToken);

    Task<ErrorOr<AuthInfoWithRefreshTokenResponse>> RefreshUserTokenAsync(
        RefreshAccessTokenRequest request, CancellationToken cancellationToken);
}