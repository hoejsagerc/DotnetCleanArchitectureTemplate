using Pokemon.Domain.Common.PokemonAggregate.ValueObjects;

namespace Pokemon.Domain.Common.PokemonAggregate.Entities;

public sealed class Stat : Entity<StatId>
{
    public string Name { get; private set; }
    public int BaseStat { get; private set; }
    public int Effort { get; private set; }

    private Stat(
        string name,
        int baseStat,
        int effort,
        StatId? id = null) : base(id ?? StatId.CreateUnique())
    {
        Name = name;
        BaseStat = baseStat;
        Effort = effort;
    }

    public static Stat Create(string name, int baseStat, int effort)
    {
        return new Stat(name, baseStat, effort);
    }
}