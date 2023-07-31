using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Pokemon.Client.Authentication;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        await Task.CompletedTask;

        return new AuthenticationState(
            new ClaimsPrincipal(
                new ClaimsIdentity()
            )
        );
    }
}
