using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Pokemon.Client.Services.v1.Authentication;

namespace Pokemon.Client.Authentication;

public class CustomCookieAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IAuthClient _authClient;

    public CustomCookieAuthenticationStateProvider(IAuthClient authClient)
    {
        _authClient = authClient;
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var authResult = await _authClient.MeAsync();
        if (authResult.Data is not null && !string.IsNullOrEmpty(authResult.Data.Email))
        {
            var nameClaim = new Claim(ClaimTypes.Name, authResult.Data.Email);
            var emailClaim = new Claim(ClaimTypes.Email, authResult.Data.Email);
            var givenNameClaim = new Claim(ClaimTypes.GivenName, authResult.Data.Firstname);
            var userIdClaim = new Claim(ClaimTypes.NameIdentifier, authResult.Data.Id.ToString());
            var claimsIdentity = new ClaimsIdentity(new[] { nameClaim, emailClaim, givenNameClaim, userIdClaim }, "serverAuth");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return new AuthenticationState(claimsPrincipal);
        }
        else
        {
            return new AuthenticationState(
                new ClaimsPrincipal(
                    new ClaimsIdentity()
                )
            );
        }
    }
}