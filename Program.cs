using System.Security.Claims;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SecurityLabs;
using SecurityLabs.Middlewares;

var builder = WebApplication
    .CreateBuilder(args);

builder.Services
    .AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(c =>
    {
        c.Authority = $"https://{builder.Configuration["AppCredentials:ClientInfo:Domain"]}/";
        c.Audience = builder.Configuration["AppCredentials:ClientInfo:Audience"];

        c.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier,

            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            
            ValidAudience = builder.Configuration["AppCredentials:ClientInfo:Audience"],
            ValidIssuer = builder.Configuration["AppCredentials:ClientInfo:Issuer"],
            ClockSkew = TimeSpan.Zero,
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
