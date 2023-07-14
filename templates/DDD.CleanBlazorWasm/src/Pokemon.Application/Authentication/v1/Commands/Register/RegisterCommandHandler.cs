using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pokemon.Application.Authentication.v1.Common;
using Pokemon.Application.Common.Interfaces.Authentication;
using Pokemon.Application.Common.Interfaces.Persistence;
using Pokemon.Domain.Common.DomainErrors;
using Pokemon.Domain.AuthenticationAggregates;
using Pokemon.Domain.AuthenticationAggregates.ValueObjects;

namespace Pokemon.Application.Authentication.v1.Commands.Register;

public class RegisterCommandHandler :
    IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ILogger<RegisterCommandHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        ILogger<RegisterCommandHandler> logger,
        IHttpContextAccessor httpContextAccessor,
        IRefreshTokenRepository refreshTokenRepository,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _refreshTokenRepository = refreshTokenRepository;
        _configuration = configuration;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken)
    {
        string? sourceIpAddress = _httpContextAccessor.HttpContext?
            .Connection.RemoteIpAddress?.ToString();

        var existingUser = await _userRepository.GetUserByEmailAsync(command.Email);
        if (existingUser is not null)
        {
            _logger.LogWarning("User registration failed - email address already exist, {@UserEmail}, {@SourceIpAddress}, {@DateTimeUtc}",
                command.Email, sourceIpAddress, DateTime.UtcNow);
            return Errors.User.DuplicateEmail;
        }

        var user = await GenerateUser(command);

        var authResult = await GenerateAuthenticationResult(user);

        _logger.LogInformation("User registered successful, {@UserEmail}, {@SourceIpAddress}, {@DateTimeUtc}",
            command.Email, sourceIpAddress, DateTime.UtcNow);

        return authResult;
    }

    private async Task<User> GenerateUser(RegisterCommand command)
    {
        var hashedPassword = PasswordHash.Hash(command.Password);
        User user = User.Create(
            command.Username,
            command.Firstname,
            command.Lastname,
            command.Email,
            hashedPassword
        );
        await _userRepository.AddAsync(user);

        return user;
    }

    private async Task<AuthenticationResult> GenerateAuthenticationResult(User user)
    {
        var refreshToken = RefreshToken.Create(
            DateTime.UtcNow.AddDays(
                _configuration.GetValue<int>("RefreshTokenSettings:ExpiryDays")),
            UserId.Create(user.Id.Value));

        await _refreshTokenRepository.AddAsync(refreshToken);

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token, refreshToken.Token);
    }
}