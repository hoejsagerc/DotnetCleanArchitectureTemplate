using Pokemon.Domain.AuthenticationAggregates;

namespace Pokemon.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}