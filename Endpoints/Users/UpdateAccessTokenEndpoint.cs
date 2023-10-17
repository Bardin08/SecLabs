using FastEndpoints;
using SecurityLabs.Contracts.Api.Models;
using SecurityLabs.Extensions;
using SecurityLabs.Services.Interfaces;

namespace SecurityLabs.Endpoints.Users;

internal class UpdateAccessTokenEndpoint
    : Endpoint<RefreshAccessTokenRequest, Response<AuthInfoWithRefreshTokenResponse>>
{
    private readonly IUsersService _usersService;

    public UpdateAccessTokenEndpoint(IUsersService usersService)
    {
        _usersService = usersService;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/api/users/token/refresh");
    }

    public override async Task HandleAsync(RefreshAccessTokenRequest req, CancellationToken ct)
    {
        var userToken = await _usersService.RefreshUserTokenAsync(req, ct);
        await SendAsync(userToken.ToResponse(), cancellation: ct);
    }
}