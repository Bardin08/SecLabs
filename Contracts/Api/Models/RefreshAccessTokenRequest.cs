using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace SecurityLabs.Contracts.Api.Models;

internal record RefreshAccessTokenRequest
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
}