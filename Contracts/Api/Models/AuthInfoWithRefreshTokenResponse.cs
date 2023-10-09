using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace SecurityLabs.Contracts.Api.Models;

internal record AuthInfoWithRefreshTokenResponse : AuthInfoResponse
{
    [JsonProperty("token_id", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("token_id")]
    public string? TokenId { get; set; }

    [JsonProperty("refresh_token", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }
}