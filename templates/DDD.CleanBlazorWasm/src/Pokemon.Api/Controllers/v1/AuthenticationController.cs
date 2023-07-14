using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pokemon.Application.Authentication.v1.Commands.Register;
using Pokemon.Application.Authentication.v1.Common;
using Pokemon.Application.Authentication.v1.Queries.Login;
using Pokemon.Application.Authentication.v1.Commands.Refresh;
using Pokemon.Contracts.v1.Authentication;
using Pokemon.Domain.Common.DomainErrors;
using System.Security.Claims;
using Pokemon.Application.Authentication.v1.Commands.Logout;

namespace Pokemon.Api.Controllers.v1;

[Route("api/v{version:apiVersion}/auth")]
[AllowAnonymous]
[ApiVersion("1.0")]
[Produces(contentType: "application/json")]
public class AuthenticationController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;


    public AuthenticationController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Register new user
    /// </summary>
    /// <param name="request"></param>
    /// <response code="201">Returns the created user with a valid token</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = _mapper.Map<RegisterCommand>(request);
        ErrorOr<AuthenticationResult> authResult = await _mediator.Send(command);

        return authResult.Match(
            authResult => Created("", _mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(errors));
    }


    /// <summary>
    /// Authenticate with existing user
    /// </summary>
    /// <param name="request"></param>
    /// <response code="200">Returns the authenticated user with a valid token</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = _mapper.Map<LoginQuery>(request);
        var authResult = await _mediator.Send(query);

        if (authResult.IsError && authResult.FirstError == Errors.Authentication.InvalidCredentials)
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: authResult.FirstError.Description);
        }

        return authResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(errors));
    }


    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: "Unauthorized");
        }

        var command = _mapper.Map<LogoutCommand>(userId);
        var result = await _mediator.Send(command);

        return result.Match(
            result => NoContent(),
            errors => Problem(errors));
    }


    /// <summary>
    /// Refresh users Jwt token
    /// </summary>
    /// <param name="request"></param>
    /// <param name="userId"></param>
    /// <response code="201">Returns the authenticated user with a new valid jwt token</response>
    [HttpPost("refresh/{userId}")]
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request, string userId)
    {
        var command = _mapper.Map<RefreshCommand>((request, userId));

        var refreshTokenResult = await _mediator.Send(command);

        return refreshTokenResult.Match(
            refreshTokenResult => Created("", _mapper.Map<AuthenticationResponse>(refreshTokenResult)),
            errors => Problem(errors));
    }
}