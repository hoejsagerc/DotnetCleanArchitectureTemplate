using System.Security.Claims;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pokemon.Application.Authentication.v1.Commands.Register;
using Pokemon.Application.Authentication.v1.Commands.UpdatePassword;
using Pokemon.Application.Authentication.v1.Commands.UpdateUser;
using Pokemon.Application.Authentication.v1.Commands.VerifyEmail;
using Pokemon.Application.Authentication.v1.Common;
using Pokemon.Application.Authentication.v1.Queries.Me;
using Pokemon.Contracts.v1.Authentication;
using Pokemon.Domain.Common.DomainErrors;

namespace Pokemon.Api.Controllers.v1.Authentication;

[Route("api/v{version:apiVersion}/auth/account")]
[AllowAnonymous]
[ApiVersion("1.0")]
[Produces(contentType: "application/json")]
public class AccountsController : ApiController
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public AccountsController(IMapper mapper, ISender mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var command = _mapper.Map<RegisterCommand>(request);
        ErrorOr<AuthenticationResult> authResult = await _mediator.Send(command);

        return authResult.Match(
            authResult => Created("", _mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(errors));
    }


    [HttpPost("verify-email")]
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
    {
        var command = _mapper.Map<VerifyEmailCommand>(request);
        ErrorOr<AuthenticationResult>authResult = await _mediator.Send(command);

        return authResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(errors));
    }


    [HttpGet("me")]
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Me()
    {
        var email = string.Empty;
        var givenName = string.Empty;
        var userId = string.Empty;

        if (User.Identity!.IsAuthenticated)
        {
            email = User.FindFirstValue(ClaimTypes.Email);
            givenName = User.FindFirstValue(ClaimTypes.GivenName);
            userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        var query = _mapper.Map<MeQuery>((email, givenName, userId));
        var result = await _mediator.Send(query);

        return result.Match(
            result => Ok(_mapper.Map<AuthenticationResponse>(result)),
            errors => Problem(errors));
    }


    [HttpPut("email")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateEmail([FromBody] UpdateUserRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (request.Email is null)
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Email must not be null");
        }
        var command = _mapper.Map<UpdateUserEmailCommand>((request, userId));
        var authResult = await _mediator.Send(command);

        return authResult.Match(
            authResult => NoContent(),
            errors => Problem(errors));
    }


    [HttpPut("password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdateUserRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (request.Password is null)
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Password must not be null");
        }
        var command = _mapper.Map<UpdateUserPasswordCommand>((request, userId));
        var authResult = await _mediator.Send(command);

        return authResult.Match(
            authResult => NoContent(),
            errors => Problem(errors));
    }
}