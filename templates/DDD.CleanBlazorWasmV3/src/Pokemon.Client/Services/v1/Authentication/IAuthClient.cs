using Pokemon.Client.Models;
using Pokemon.Contracts.v1.Authentication;

namespace Pokemon.Client.Services.v1.Authentication;

public interface IAuthClient
{
    Task<ServiceResponse<AuthenticationResponse>> LoginAsync(LoginRequest body);
    Task<ServiceResponse<AuthenticationResponse>> MeAsync();
    Task<ServiceResponse<AuthenticationResponse>> RegisterAsync(RegisterRequest body);
    Task<ServiceResponse<string>> LogoutAsync();
    Task<ServiceResponse<AuthenticationResponse>> VerifyEmailAsync(VerifyEmailRequest body);
}