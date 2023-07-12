using ErrorOr;
using Pokemon.Domain.Common.DomainErrors;
using Pokemon.Domain.Common.Models.Identities;

namespace Pokemon.Domain.AuthenticationAggregates.ValueObjects;

public sealed class UserId : AggregateRootId<Guid>
{
    private UserId(Guid value) : base(value)
    {

    }

    public static UserId CreateUnique()
    {
        return new UserId(Guid.NewGuid());
    }

    public static UserId Create(Guid userId)
    {
        return new UserId(userId);
    }

    public static ErrorOr<UserId> Create(string userId)
    {
        if(!Guid.TryParse(userId, out var guid))
        {
            return Errors.User.InvalidUserId;
        }

        return new UserId(guid);
    }
}