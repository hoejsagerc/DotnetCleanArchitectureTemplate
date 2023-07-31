using ErrorOr;
using Pokemon.Domain.Common.Models.Identities;
using Pokemon.Domain.Common.DomainErrors;

namespace Pokemon.Domain.AuthenticationAggregates.ValueObjects;

public sealed class UserRoleClaimId : AggregateRootId<Guid>
{
    public UserRoleClaimId(Guid value) : base(value)
    {

    }

    public static UserRoleClaimId CreateUnique()
    {
        return new UserRoleClaimId(Guid.NewGuid());
    }

    public static UserRoleClaimId Create(Guid userRoleClaimId)
    {
        return new UserRoleClaimId(userRoleClaimId);
    }

    public static ErrorOr<UserRoleClaimId> Create(string userRoleClaimId)
    {
        if (!Guid.TryParse(userRoleClaimId, out var guid))
        {
            return Errors.UserRoleClaim.InvalidUserRoleClaimId;
        }

        return new UserRoleClaimId(guid);
    }
}