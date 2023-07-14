using ErrorOr;
using Pokemon.Domain.Common.DomainErrors;
using Pokemon.Domain.Common.Models.Identities;

namespace Pokemon.Domain.Common.PokemonAggregate.ValueObjects;

public sealed class PocketMonsterId : AggregateRootId<Guid>
{
    private PocketMonsterId(Guid value) : base(value)
    {
    }

    public static PocketMonsterId CreateUnique()
    {
        return new PocketMonsterId(Guid.NewGuid());
    }

    public static PocketMonsterId Create(Guid value)
    {
        return new PocketMonsterId(value);
    }

    public static ErrorOr<PocketMonsterId> Create(string value)
    {
        if (!Guid.TryParse(value, out var guid))
        {
            return Errors.PocketMonster.InvalidPokemonId;
        }

        return new PocketMonsterId(guid);
    }
}