using System.Security.Claims;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SecurityLabs;
using SecurityLabs.Middlewares;

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

builder.Services.AddMemoryCache();

builder.AddPresentationLayerServices();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ReValidateJwtMiddleware>();

app.UseFastEndpoints();

app.Run();
