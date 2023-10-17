using System.Text.Json.Serialization;
using Newtonsoft.Json;
using SecurityLabs.Contracts.Api.Models;

namespace SecurityLabs.Contracts.Api.External;

internal class Auth0CreateUserRequest
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

    [JsonProperty("connection")]
    [JsonPropertyName("connection")]
    public string Connection => "Username-Password-Authentication";

    internal static Auth0CreateUserRequest FromCreateUserRequest(CreateUserRequest request)
    {
        return new Auth0CreateUserRequest
        {
            UserId = request.UserId,
            Username = request.Username,
            AppMetadata = request.AppMetadata,
            EmailVerified = request.EmailVerified,
            Email = request.Email,
            UserMetadata = request.UserMetadata,
            Blocked = request.Blocked,
            Password = request.Password,
        };
    }
}