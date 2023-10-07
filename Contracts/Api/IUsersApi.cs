using Microsoft.AspNetCore.Mvc;
using Refit;
using SecurityLabs.Contracts.Api.Models;

namespace SecurityLabs.Contracts.Api;

internal interface IUsersApi
{
    [HttpPost("/api/v2/users")]
    Task<ApiResponse<UserInfoResponse>> CreateUserAsync(CreateUserRequest request,
        [HeaderCollection] IDictionary<string, string> headers,
        CancellationToken cancellationToken);
}