using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Pokemon.Application.Authentication.v1.Common;
using Pokemon.Domain.AuthenticationAggregate;

namespace Pokemon.Application.Authentication.v1.Queries.Me;

public class MeQueryHandler : IRequestHandler<MeQuery, ErrorOr<AuthenticationResult>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public MeQueryHandler(
        UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(MeQuery query, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(query.Email);

        if (user is not null)
        {

            return new AuthenticationResult(
                user,
                "");
            // if (user.GivenName == query.GivenName && user.Id == query.UserId)
            // {
            //     return new AuthenticationResult(
            //         user,
            //         "");
            // }
        }

        user = new ApplicationUser();

        return new AuthenticationResult(
            user,
            "");
    }
}
