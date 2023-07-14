using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Pokemon.Client.Authentication;

public class JwtTokenHandler : DelegatingHandler
{

    private readonly ILocalStorageService _localStorage;

    public JwtTokenHandler(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.RequestUri!.AbsolutePath.ToLower().Contains("/auth/login") ||
            request.RequestUri.AbsolutePath.ToLower().Contains("/auth/register"))
        {
            return await base.SendAsync(request, cancellationToken);
        }

        var token = (await _localStorage.GetItemAsStringAsync("jwtToken")).Replace("\"", "");
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Add("Authorization", $"Bearer {token}");
        }


        return await base.SendAsync(request, cancellationToken);

    }
}