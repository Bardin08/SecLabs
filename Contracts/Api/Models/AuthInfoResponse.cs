using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace SecurityLabs.Contracts.Api.Models;

internal record AuthInfoResponse
{
    [JsonProperty("access_token")]
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; set; }
    [JsonProperty("token_type")]
    [JsonPropertyName("token_type")]
    public required string TokenType { get; set; }
    [JsonProperty("expires_in")]
    [JsonPropertyName("expires_in")]
    public required int ExpiresIn { get; set; }
}