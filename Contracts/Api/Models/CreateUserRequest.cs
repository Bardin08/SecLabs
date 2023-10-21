using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace SecurityLabs.Contracts.Api.Models;

internal record CreateUserRequest
{
    [JsonProperty("email")]
    [JsonPropertyName("email")]
    public string? Email { get; init; }

    [JsonProperty("user_metadata")]
    [JsonPropertyName("user_metadata")]
    public UserMetadata? UserMetadata { get; init; }

    [JsonProperty("blocked")]
    [JsonPropertyName("blocked")]
    public bool Blocked { get; init; }

    [JsonProperty("email_verified")]
    [JsonPropertyName("email_verified")]
    public bool EmailVerified { get; init; }

    [JsonProperty("app_metadata")]
    [JsonPropertyName("app_metadata")]
    public AppMetadata? AppMetadata { get; init; }

    [JsonProperty("user_id")]
    [JsonPropertyName("user_id")]
    public string? UserId { get; init; }

    [JsonProperty("password")]
    [JsonPropertyName("password")]
    public string? Password { get; init; }

    [JsonProperty("username")]
    [JsonPropertyName("username")]
    public string? Username { get; init; }
}