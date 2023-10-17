using System.Security.Claims;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using SecurityLabs;

var builder = WebApplication
    .CreateBuilder(args);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(c =>
    {
        c.Authority = $"https://{builder.Configuration["AppCredentials:ClientInfo:Domain"]}/";
        c.Audience = builder.Configuration["AppCredentials:ClientInfo:Audience"];

        c.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });

builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

builder.AddPresentationLayerServices();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();

app.Run();

public class HasScopeRequirement : IAuthorizationRequirement
{
    public string Issuer { get; }
    public string Scope { get; }

    public HasScopeRequirement(string scope, string issuer)
    {
        Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
    }
}

public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        HasScopeRequirement requirement)
    {
        if (!context.User.HasClaim(c => c.Type == "scope" && c.Issuer == requirement.Issuer))
        {
            return Task.CompletedTask;
        }

        var scopesClaim = context.User
            .FindFirst(c => c.Type is "scope" &&
                            c.Issuer == requirement.Issuer)!;

        var scopes = scopesClaim.Value.Split(' ');

        if (scopes.Any(s => s == requirement.Scope))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}