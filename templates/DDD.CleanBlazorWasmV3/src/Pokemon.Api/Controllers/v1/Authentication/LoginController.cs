using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Pokemon.Application.Authentication.v1.Commands.Logout;
using Pokemon.Application.Authentication.v1.Queries.Login;
using Pokemon.Contracts.v1.Authentication;
using Pokemon.Domain.AuthenticationAggregate;
using Pokemon.Domain.Common.DomainErrors;
using Pokemon.Infrastructure.Authentication;

namespace Pokemon.Api.Controllers.v1.Authentication;

[Route("api/v{version:apiVersion}/auth")]
[AllowAnonymous]
[ApiVersion("1.0")]
[Produces(contentType: "application/json")]
public class LoginController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public LoginController(
        ISender mediator,
        IMapper mapper,
        SignInManager<ApplicationUser> signInManager)
    {
        _mediator = mediator;
        _mapper = mapper;
        _signInManager = signInManager;
    }


    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var query = _mapper.Map<LoginQuery>(request);

        var authResult = await _mediator.Send(query);

        if(authResult.IsError && authResult.FirstError == Errors.User.InvalidCredentials)
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: authResult.FirstError.Description);
        }

        var user = authResult.Value.User;

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
        };

        var claimsIdentity = new ClaimsIdentity(claims, "serverAuth");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await HttpContext.SignInAsync(claimsPrincipal);

        return authResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(errors));
    }


    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(userId is null)
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: "Unauthorized");
        }

        var command = _mapper.Map<LogoutCommand>(userId);
        var result = await _mediator.Send(command);

        // sign out with jwt auth
        await _signInManager.SignOutAsync();
        // sign out with cookie auth
        await HttpContext.SignOutAsync();

        if (!result.IsError)
        {
            return NoContent();
        }
        else
        {
            return Problem(result.Errors);
        }
    }
}