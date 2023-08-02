using System.ComponentModel.DataAnnotations;

namespace Pokemon.Contracts.v1.Authentication;

public class VerifyEmailRequest
{
    [Required]
    [Display(Name = "UserId")]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Verification Code")]
    public string Code { get; set; } = string.Empty;
}