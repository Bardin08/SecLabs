using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace SecurityLabs.Contracts.Api.Models;

public record ChangeUserPasswordRequest
{
    [JsonProperty("user_id")]
    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }

    [JsonProperty("password")]
    [JsonPropertyName("password")]
    public required string Password { get; set; }

    [JsonProperty("connection")]
    [JsonPropertyName("connection")]
    public string Connection => "Username-Password-Authentication";
}