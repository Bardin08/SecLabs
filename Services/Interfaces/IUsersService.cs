using ErrorOr;
using SecurityLabs.Contracts.Api.Models;

namespace SecurityLabs.Services.Interfaces;

public interface IUsersService
{
    Task<ErrorOr<UserInfoResponse>> CreateUserAsync(
        CreateUserRequest request, CancellationToken cancellationToken);
}