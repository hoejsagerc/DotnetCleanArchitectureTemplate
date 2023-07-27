namespace Pokemon.Contracts.v1.Authentication;

/// <summary>
/// Contract for registering new user
/// </summary>
public record RegisterRequest
{
    /// <summary>
    /// The Users username
    /// </summary>
    /// <example>j.doe</example>
    public string Username { get; set; } = string.Empty;


    /// <summary>
    /// The Users Firstnanme
    /// </summary>
    /// <example>John</example>
    public string Firstname { get; set; } = string.Empty;


    /// <summary>
    /// The Users Lastname
    /// </summary>
    /// <example>Doe</example>
    public string Lastname { get; set; } = string.Empty;


    /// <summary>
    /// The Users Email address
    /// </summary>
    /// <example>j.doe@mail.com</example>
    public string Email { get; set; } = string.Empty;


    /// <summary>
    /// The Users password
    /// </summary>
    /// <example>MySuperPasswor123</example>
    public string Password { get; set; } = string.Empty;
}
