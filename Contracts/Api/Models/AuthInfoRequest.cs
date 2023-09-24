using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace SecurityLabs.Contracts.Api.Models;

internal record AuthInfoRequest
{
    [JsonProperty("client_id")]
    [JsonPropertyName("client_id")]
    public required string ClientId { get; set; }
    [JsonProperty("client_secret")]
    [JsonPropertyName("client_secret")]
    public required string ClientSecret { get; set; }
    [JsonProperty("grant_type")]
    [JsonPropertyName("grant_type")]
    public required string GrantType { get; set; }
    [JsonProperty("audience")]
    [JsonPropertyName("audience")]
    public required string Audience { get; set; }
}