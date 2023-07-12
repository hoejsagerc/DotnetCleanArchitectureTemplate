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
    /// <response code="200">Returns the created user with a valid token</response>
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = _mapper.Map<RegisterCommand>(request);
        ErrorOr<AuthenticationResult> authResult = await _mediator.Send(command);

        return authResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(errors));
    }


    /// <summary>
    /// Authenticate with existing user
    /// </summary>
    /// <param name="request"></param>
    /// <response code="200">Returns the authenticated user with a valid token</response>
    /// <response code="401">Returns Unauthorized if the credentials is invalid</response>
    /// <response code="404">Returns NotFound if the user does not exist</response>
    [HttpPost("login")]
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


    /// <summary>
    /// Refresh users Jwt token
    /// </summary>
    /// <param name="request"></param>
    /// <param name="userId"></param>
    /// <response code="200">Returns the authenticated user with a new valid jwt token</response>
    /// <response code="404">Return not found if either the user does not exist or the refresh token does not exist</response>
    [HttpPost("refresh/{userId}")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request, string userId)
    {
        var command = _mapper.Map<RefreshCommand>((request, userId));

        var refreshTokenResult = await _mediator.Send(command);

        return refreshTokenResult.Match(
            refreshTokenResult => Ok(_mapper.Map<AuthenticationResponse>(refreshTokenResult)),
            errors => Problem(errors));
    }
}