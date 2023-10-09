using FastEndpoints;
using SecurityLabs.Contracts.Api.Models;
using SecurityLabs.Extensions;
using SecurityLabs.Services.Interfaces;

namespace SecurityLabs.Endpoints.Users;

internal class GetUserTokenEndpoint : Endpoint<GetUserTokenRequest, Response<AuthInfoWithRefreshTokenResponse>>
{
    private readonly IUsersService _usersService;

    public GetUserTokenEndpoint(IUsersService usersService)
    {
        _usersService = usersService;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/api/users/token");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetUserTokenRequest req, CancellationToken ct)
    {
        var userToken = await _usersService.GetUserTokenAsync(req, ct);
        await SendAsync(userToken.ToResponse(), cancellation: ct);
    }
}