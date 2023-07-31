using ErrorOr;
using Pokemon.Domain.Common.Models.Identities;
using Pokemon.Domain.Common.DomainErrors;

namespace Pokemon.Domain.AuthenticationAggregates.ValueObjects;


public sealed class UserRoleId : AggregateRootId<Guid>
{
    public UserRoleId(Guid value) : base(value)
    {

    }

    public static UserRoleId CreateUnique()
    {
        return new UserRoleId(Guid.NewGuid());
    }

    public static UserRoleId Create(Guid userRoleId)
    {
        return new UserRoleId(userRoleId);
    }

    public static ErrorOr<UserRoleId> Create(string userRoleId)
    {
        if (!Guid.TryParse(userRoleId, out var guid))
        {
            return Errors.UserRole.InvalidUserRoleId;
        }

        return new UserRoleId(guid);
    }
}