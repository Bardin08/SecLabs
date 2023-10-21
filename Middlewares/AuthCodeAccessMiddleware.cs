using System.Net;
using System.Text;
using Microsoft.Extensions.Primitives;
using SecurityLabs.Contracts.Api.Models;
using SecurityLabs.Services.Proxies.Interfaces;

namespace SecurityLabs.Middlewares;

internal class AuthCodeAccessMiddleware
{
    private readonly IAccessTokenServiceProxy _accessTokenService;
    private readonly RequestDelegate _next;

    public AuthCodeAccessMiddleware(
        IAccessTokenServiceProxy accessTokenService,
        RequestDelegate next)
    {
        _accessTokenService = accessTokenService;
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Query.TryGetValue("code", out var code))
        {
            var request = new GetAuthInfoFromCodeRequest()
            {
                Code = code.FirstOrDefault()
            };

            var authInfo = await _accessTokenService.GetAuthInfoFromCodeAsync(request, CancellationToken.None);
            if (authInfo.IsError)
            {
                const string error = "Invalid Auth0 Authorization code. Try to login again later";
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(error));
                return;
            }

            context.Request.Headers.Authorization =
                new StringValues($"Bearer {authInfo.Value.AccessToken}");

            await _next(context);
        }
        
        await _next(context);
    }
}