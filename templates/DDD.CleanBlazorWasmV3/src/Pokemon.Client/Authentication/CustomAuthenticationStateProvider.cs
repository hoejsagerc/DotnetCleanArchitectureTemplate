using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Pokemon.Client.Services.v1.Authentication;

namespace Pokemon.Client.Authentication;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IAuthClient _authClient;

    public CustomAuthenticationStateProvider(IAuthClient authClient)
    {
        _authClient = authClient;
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var authResult = await _authClient.MeAsync();
        if (authResult.Data is not null && !string.IsNullOrEmpty(authResult.Data.Email))
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, authResult.Data.Email),
                new Claim(ClaimTypes.Name, authResult.Data.Email!),
                new Claim(ClaimTypes.NameIdentifier, authResult.Data.Id!)
            };
            var claimsIdentity = new ClaimsIdentity(claims, "serverAuth");
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
