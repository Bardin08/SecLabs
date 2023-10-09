using System.Text.Json.Serialization;
using FastEndpoints;
using Newtonsoft.Json;
using Refit;

namespace SecurityLabs.Contracts.Api.Models;

public record GetUserTokenRequest
{
    [FromHeader(HeaderName = "Authorization", RemoveFromSchema = true, IsRequired = false)]
    public string? AuthToken { get; set; }

    [JsonProperty("grant_type")]
    [JsonPropertyName("grant_type")]
    public required string GrantType { get; set; }

    [JsonProperty("username")]
    [JsonPropertyName("username")]
    public required string Username { get; set; }

    [JsonProperty("password")]
    [JsonPropertyName("password")]
    public required string Password { get; set; }

    [JsonProperty("client_id")]
    [JsonPropertyName("client_id")]
    public string? ClientId { get; set; }

    [JsonProperty("client_secret")]
    [JsonPropertyName("client_secret")]
    public string? ClientSecret { get; set; }

    [JsonProperty("audience")]
    [JsonPropertyName("audience")]
    public string? Audience { get; set; }

    [JsonProperty("scope")]
    [JsonPropertyName("scope")]
    public string? Scope { get; set; }
}