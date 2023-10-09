using System.Text.Json.Serialization;
using Newtonsoft.Json;
using SecurityLabs.Contracts.Api.Models;
using SecurityLabs.Endpoints.Users;

namespace SecurityLabs.Contracts.Api.External;

internal record Auth0RefreshAccessTokenRequest
{
    [JsonProperty("grant_type")]
    [JsonPropertyName("grant_type")]
    public string GrantType => "refresh_token";

    [JsonProperty("refresh_token")]
    [JsonPropertyName("refresh_token")]
    public required string RefreshToken { get; init; }

    [JsonProperty("scope")]
    [JsonPropertyName("scope")]
    public string? Scope { get; init; }

    [JsonProperty("client_id")]
    [JsonPropertyName("client_id")]
    public string? ClientId { get; init; }

    [JsonProperty("client_secret")]
    [JsonPropertyName("client_secret")]
    public string? ClientSecret { get; init; }

    public static Auth0RefreshAccessTokenRequest FromRefreshTokenRequest(
        RefreshAccessTokenRequest req,
        string clientId,
        string clientSecret)
    {
        return new Auth0RefreshAccessTokenRequest
        {
            Scope = req.Scope,
            RefreshToken = req.RefreshToken,
            ClientId = clientId,
            ClientSecret = clientSecret
        };
    }
}