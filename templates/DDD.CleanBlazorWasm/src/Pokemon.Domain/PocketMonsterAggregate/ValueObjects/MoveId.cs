using ErrorOr;
using Pokemon.Domain.Common.DomainErrors;
using Pokemon.Domain.Common.Models.Identities;

namespace Pokemon.Domain.Common.PokemonAggregate.ValueObjects;

public sealed class MoveId : EntityId<Guid>
{
    public MoveId(Guid value) : base(value) 
    { 

    }

    public static MoveId CreateUnique()
    {
        return new MoveId(Guid.NewGuid());
    }

    public static MoveId Create(Guid value)
    {
        return new MoveId(value);
    }

    public static ErrorOr<MoveId> Create(string value)
    {
        if (!Guid.TryParse(value, out var guid))
        {
            return Errors.PocketMonster.InvalidPokemonId;
        }

        return new MoveId(guid);
    }
}