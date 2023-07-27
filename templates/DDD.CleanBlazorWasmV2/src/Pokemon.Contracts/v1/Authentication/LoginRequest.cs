using System.Text.Json.Serialization;

namespace Pokemon.Contracts.v1.Authentication;


/// <summary>
/// Contract for sending a Login request
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// The email address of the User
    /// </summary>
    /// <example>j.doe@mail.com</example>
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;


    /// <summary>
    /// The Users password
    /// </summary>
    /// <example>MySuperPassword123</example>
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}