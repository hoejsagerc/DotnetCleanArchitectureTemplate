using ErrorOr;
using Pokemon.Domain.Common.Models.Identities;
using Pokemon.Domain.Common.DomainErrors;

namespace Pokemon.Domain.UserAggregate.ValueObjects;

public sealed class RefreshTokenId : AggregateRootId<Guid>
{
    public RefreshTokenId(Guid value) : base(value)
    {

    }

    public static RefreshTokenId CreateUnique()
    {
        return new RefreshTokenId(Guid.NewGuid());
    }

    public static RefreshTokenId Create(Guid refreshTokenId)
    {
        return new RefreshTokenId(refreshTokenId);
    }

    public static ErrorOr<RefreshTokenId> Create(string refreshTokenId)
    {
        if (!Guid.TryParse(refreshTokenId, out var guid))
        {
            return Errors.RefreshToken.InvalidRefreshTokenId;
        }

        return new RefreshTokenId(guid);
    }
}