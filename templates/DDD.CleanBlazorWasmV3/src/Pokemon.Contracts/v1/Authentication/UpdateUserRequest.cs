using System.ComponentModel.DataAnnotations;

namespace Pokemon.Contracts.v1.Authentication;

public class UpdateUserRequest
{
    [EmailAddress]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "Users Given name")]
    public string? GivenName { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "Users Surname")]
    public string? Surname { get; set; }

    [StringLength(100, ErrorMessage = "The {0} must be at lease {2} and max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string? Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [Compare("Password", ErrorMessage = "The password and confirmations passwords do not match.")]
    public string? ConfirmPassword { get; set; }
}