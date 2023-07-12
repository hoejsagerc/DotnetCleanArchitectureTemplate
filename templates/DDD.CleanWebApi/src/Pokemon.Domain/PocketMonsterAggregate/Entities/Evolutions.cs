using Pokemon.Domain.Common.PokemonAggregate.ValueObjects;

namespace Pokemon.Domain.Common.PokemonAggregate.Entities;

public sealed class Evolutions : Entity<EvolutionId>
{
    public string Name { get; private set; }

    private Evolutions(string name, EvolutionId? id = null) : base(id ?? EvolutionId.CreateUnique())
    {
        Name = name;
    }

    public static Evolutions Create(string name)
    {
        return new Evolutions(name);
    }
}