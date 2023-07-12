using Pokemon.Domain.Common.Models;
using Pokemon.Domain.Common.PokemonAggregate.Entities;
using Pokemon.Domain.Common.PokemonAggregate.ValueObjects;
using Pokemon.Domain.PokemonAggregate.Events;
using Pokemon.Domain.UserAggregate.ValueObjects;

namespace Pokemon.Domain.PokemonAggregate;

public sealed class PocketMonster : AggregateRoot<PokemonId, Guid>, IAuditableEntity
{
    private readonly List<Move> _moves = new();
    private readonly List<Stat> _stats = new();
    private readonly List<Ability> _abilities = new();
    private readonly List<Evolutions> _evolutions = new();

    public string Name { get; private set; }
    public string Type { get; private set; }
    public int Height { get; private set; }
    public int Weight { get; private set; }
    public int Level { get; private set; }
    public int Experience { get; private set; }
    public int ExperienceToNextLevel { get; private set; }
    public string Sprite { get; private set; }
    public UserId? UserId { get; private set; }
    public IReadOnlyList<Move> Moves => _moves.AsReadOnly();
    public IReadOnlyList<Stat> Stats => _stats.AsReadOnly();
    public IReadOnlyList<Ability> Abilities => _abilities.AsReadOnly();
    public IReadOnlyList<Evolutions> Evolutions => _evolutions.AsReadOnly();

    public DateTime CreatedOnUtc { get; set; }
    public DateTime ModifiedOnUtc {get; set; }

    private PocketMonster(
        PokemonId pokemonId,
        string name,
        string type,
        int height,
        int weight,
        int level,
        int experience,
        int experienceToNextLevel,
        string sprite,
        UserId userId,
        List<Move> moves,
        List<Stat> stats,
        List<Ability> abilities,
        List<Evolutions> evolutions
    ) : base(pokemonId)
    {
        Name = name;
        Type = type;
        Height = height;
        Weight = weight;
        Level = level;
        Experience = experience;
        ExperienceToNextLevel = experienceToNextLevel;
        Sprite = sprite;
        UserId = userId;
        _moves = moves;
        _stats = stats;
        _abilities = abilities;
        _evolutions = evolutions;
    }


    /// <summary>
    /// Method for creating a new Pokemon object
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <param name="height"></param>
    /// <param name="weight"></param>
    /// <param name="level"></param>
    /// <param name="baseExperience"></param>
    /// <param name="sprite"></param>
    /// <param name="userId"></param>
    /// <param name="moves"></param>
    /// <param name="stats"></param>
    /// <param name="abilities"></param>
    /// <param name="evolutions"></param>
    /// <returns>A Pokemon object</returns>
    public static PocketMonster Create(
        string name,
        string type,
        int height,
        int weight,
        int level,
        int experience,
        int experienceToNextLevel,
        string sprite,
        UserId userId,
        List<Move>? moves = null,
        List<Stat>? stats = null,
        List<Ability>? abilities = null,
        List<Evolutions>? evolutions = null
    )
    {
        var pokemon = new PocketMonster(
            PokemonId.CreateUnique(),
            name,
            type,
            height,
            weight,
            level,
            experience,
            experienceToNextLevel,
            sprite,
            userId,
            moves ?? new(),
            stats ?? new(),
            abilities ?? new(),
            evolutions ?? new());

        return pokemon;
    }


    public void Update(
        string name,
        string type,
        int height,
        int weight,
        int level,
        string sprite)
    {
        Name = name;
        Type = type;
        Height = height;
        Weight = weight;
        Level = level;
        Sprite = sprite;

        AddDomainEvent(new PokemonUpdated(this));
    }


    public void TransferToUser(
        UserId userId)
    {
        var oldUserId = UserId;

        UserId = userId;

        AddDomainEvent(new PokemonTransfered(this, oldUserId!));
    }


    public void AddExperience(
        int experience)
    {
        if (Level != 100)
        {
            CheckPokemonLevel(experience);
        }
    }


    private void CheckPokemonLevel(int experience)
    {
        const double ExperienceRatio = 1.5;
        const int ExperienceStartingValue = 10000;

        int newExp = Experience + experience;

        if (newExp > ExperienceToNextLevel)
        {
            var restExperience = newExp - ExperienceToNextLevel;

            Level++;
            ExperienceToNextLevel = (int)(Level * ExperienceStartingValue * ExperienceRatio);
            Experience = restExperience;
        }
        else
        {
            Experience = newExp;
        }
    }


#pragma warning disable CS8618
    private PocketMonster()
    {
    }
#pragma warning restore CS8618
}
