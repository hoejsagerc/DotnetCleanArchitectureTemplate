using Pokemon.Domain.AuthenticationAggregate;

namespace Pokemon.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken (ApplicationUser user);
}