using System.Text.Json.Serialization;
using FastEndpoints;
using Newtonsoft.Json;

namespace SecurityLabs.Contracts.Api.Models;

[HideFromDocs]
public class GetAuthInfoFromCodeRequest
{
    [JsonProperty("client_id")]
    [JsonPropertyName("client_id")]
    public string? ClientId { get; set; }

    [JsonProperty("client_secret")]
    [JsonPropertyName("client_secret")]
    public string? ClientSecret { get; set; }

    [JsonProperty("audience")]
    [JsonPropertyName("audience")]
    public string? Audience { get; set; }

    [JsonProperty("code")]
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonProperty("grant_type")]
    [JsonPropertyName("grant_type")]
    public string GrantType => "authorization_code";

    [JsonProperty("redirect_uri")]
    [JsonPropertyName("redirect_uri")]
    public string RedirectUri => "http://localhost:5000/api/login/status";
}