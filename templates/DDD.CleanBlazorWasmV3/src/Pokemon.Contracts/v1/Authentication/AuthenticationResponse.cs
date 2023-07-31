namespace Pokemon.Contracts.v1.Authentication;


/// <summary>
/// Response when either registering a user or performin login
/// </summary>
/// <param>UserName</param>
/// <param>Email</param>
/// <param>AccessToken</param>
public class AuthenticationResponse
{
    /// <summary>
    /// The users UserName
    /// </summary>
    /// <example>user@example.com</example>
    public string? UserName { get; set; }

    /// <summary>
    /// The users Email
    /// </summary>
    /// <example>user@example.com</example>
    public string? Email { get; set; }

    /// <summary>
    /// Authorized Jwt Access Token
    /// </summary>
    /// <example>string</example>
    public string? AccessToken { get; set; }
}