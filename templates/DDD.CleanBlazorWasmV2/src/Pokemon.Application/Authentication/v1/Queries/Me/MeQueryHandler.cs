using System.Data.Common;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Pokemon.Application.Authentication.v1.Common;
using Pokemon.Application.Common.Interfaces.Persistence;
using Pokemon.Domain.AuthenticationAggregates;
using Pokemon.Domain.AuthenticationAggregates.ValueObjects;
using Pokemon.Domain.Common.DomainErrors;

namespace Pokemon.Application.Authentication.v1.Queries.Me;

public class MeQueryHandler : IRequestHandler<MeQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<MeQueryHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MeQueryHandler(
        IUserRepository userRepository,
        ILogger<MeQueryHandler> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        MeQuery query,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(query.Email);
        if (user is not null)
        {
            var dbUserId = user.Id.ToString();

            if (user.Firstname == query.GivenName && query.Id == dbUserId)
            {
                return new AuthenticationResult(
                    user,
                    "",
                    "");
            }
        }

        user = User.Create(
            username: "",
            firstname: "",
            lastname: "",
            email: "",
            hashedPassword: "",
            personalData: null,
            roleClaimIds: new());

        return new AuthenticationResult(
            user,
            "",
            "");
    }
}