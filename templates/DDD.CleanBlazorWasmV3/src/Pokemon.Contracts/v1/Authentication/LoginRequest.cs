using System.ComponentModel.DataAnnotations;

namespace Pokemon.Contracts.v1.Authentication;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; } = false;
}