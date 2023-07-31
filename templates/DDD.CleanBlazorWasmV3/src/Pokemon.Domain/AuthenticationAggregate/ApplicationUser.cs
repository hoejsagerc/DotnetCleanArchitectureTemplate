
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace Pokemon.Domain.AuthenticationAggregate;

public class ApplicationUser : IdentityUser
{
    [PersonalData]
    public string? Country { get; set; }

    [PersonalData]
    public string? StreetAddress { get; set; }

    [PersonalData]
    public string? Zip { get; set; }

    [PersonalData]
    public string? City { get; set; }

    [PersonalData]
    public string? Gender { get; set; }

    [PersonalData]
    public DateTime? BirthDay { get; set; }

    [PersonalData]
    public string? GivenName { get; set; }

    [PersonalData]
    public string? Surname { get; set; }
}