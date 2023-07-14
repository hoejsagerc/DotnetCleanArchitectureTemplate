using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using Pokemon.Client.Services.v1;

namespace Pokemon.Client.Authentication;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly ApiClientV1 _client;
    private readonly NavigationManager _navigationManager;


    public CustomAuthenticationStateProvider(
        ILocalStorageService localStorage,
        ApiClientV1 client,
        NavigationManager navigationManager)
    {
        _localStorage = localStorage;
        _client = client;
        _navigationManager = navigationManager;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var jwtToken = await _localStorage.GetItemAsStringAsync("jwtToken");
        var identity = new ClaimsIdentity();

        if (!string.IsNullOrEmpty(jwtToken))
        {
            string userId = GetSubjectFromJwt(jwtToken);
            DateTime tokenExpiration = GetTokenExpiration(jwtToken);

            if (tokenExpiration < DateTime.UtcNow && !string.IsNullOrEmpty(userId))
            {
                jwtToken = await RefreshToken(userId);
            }

            if (string.IsNullOrEmpty(jwtToken))
            {
                identity = new ClaimsIdentity();
            }
            else {
                identity = new ClaimsIdentity(ParseClaimsFromJwt(jwtToken), "jwt");
            }
        }

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }

    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!));
    }

    private static Byte[] ParseBase64WithoutPadding(string base64)
    {
        switch(base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }

        return Convert.FromBase64String(base64);
    }

    public static DateTime GetTokenExpiration(string jwt)
    {
        var token = ParseClaimsFromJwt(jwt);
        var expirationClaim = token.FirstOrDefault(claim => claim.Type == "exp");
        if (expirationClaim != null && long.TryParse(expirationClaim.Value, out var expirationTime))
        {
            var expirationDateTime = DateTimeOffset.FromUnixTimeSeconds(expirationTime).UtcDateTime;
            return expirationDateTime;
        }

        throw new ArgumentException("Invalid or missing expiration claim in JWT token.");
    }

    public static string GetSubjectFromJwt(string jwt)
    {
        var token = ParseClaimsFromJwt(jwt);
        var subjectClaim = token.FirstOrDefault(claim => claim.Type == "sub");
        if (subjectClaim != null)
        {
            return subjectClaim.Value;
        }

        throw new ArgumentException("Invalid or missing subject claim in JWT token.");
    }

    private async Task<string> RefreshToken(string userId)
    {
        string refreshToken = await _localStorage.GetItemAsStringAsync("refreshToken");
        RefreshTokenRequest refreshTokenRequest = new()
            { RefreshToken = refreshToken.Replace("\"", "") };

        try {
            var authResult = await _client.RefreshAsync(userId, refreshTokenRequest);
            await _localStorage.SetItemAsync("jwtToken", authResult.Token);
            return authResult.Token;
        }
        catch {
            await _localStorage.RemoveItemAsync("jwtToken");
            await _localStorage.RemoveItemAsync("refreshToken");
            _navigationManager.NavigateTo("/login");
            return string.Empty;
        }
    }
}
