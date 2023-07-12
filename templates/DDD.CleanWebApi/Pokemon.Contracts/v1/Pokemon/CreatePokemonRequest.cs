namespace Pokemon.Contracts.v1.Pokemon;

/// <summary>
/// Contract for creating a new Pokemon
/// </summary>
/// <param name="Name">
/// <summary>The name of the Pokemon</summary>
/// <example>Pikachu</example>
/// </param>
/// <param name="Type">
/// <summary>The type of the Pokemon</summary>
/// <example>Electric</example>
/// </param>
/// <param name="Height">
/// <summary>The height of the Pokemon</summary>
/// <example>20</example>
/// </param>
/// <param name="Weight">
/// <summary>The weight of the Pokemon</summary>
/// <example>10</example>
/// </param>
/// <param name="Level">
/// <summary>The level of the Pokemon</summary>
/// <example>5</example>
/// </param>
/// <param name="Experience">
/// <summary>The current experience points of the Pokemon</summary>
/// <example>0</example>
/// </param>
/// <param name="ExperienceToNextLevel">
/// <summary>The experience points needed for the Pokemon to gain next level</summary>
/// <example>15000</example>
/// </param>
/// <param name="Sprite">
/// <summary>The Pokemon sprite represented in text</summary>
/// <example>12k312kl3m12lk3m21kl3m12k</example>
/// </param>
/// <param name="Moves"></param>
/// <param name="Stats"></param>
/// <param name="Abilities"></param>
/// <param name="Evolutions"></param>
public record CreatePokemonRequest
(
    string Name,
    string Type,
    int Height,
    int Weight,
    int Level,
    int Experience,
    int ExperienceToNextLevel,
    string Sprite,
    List<CreateMove> Moves,
    List<CreateStat> Stats,
    List<CreateAbility> Abilities,
    List<CreateEvolution> Evolutions
);

/// <summary>
/// Contract for creting a new Move
/// </summary>
/// <param name="Name">
/// <summary>The name of the move</summary>
/// <example>Double Punch</example>
/// </param>
/// <param name="Description">
/// <summary>The description of the move</summary>
/// <example>Double punch the oponent with higher chance of critical hit</example>
/// </param>
/// <param name="Damage">
/// <summary>The damage points inflicted to the oponent</summary>
/// <example>55</example>
/// </param>
/// <param name="CriticalHitFactor">
/// <summary>The factor in which the damage will be multiplied when critical hit occurs</summary>
/// <example>2.3</example>
/// </param>
/// <param name="CriticalHitChance">
/// <summary>The chance of a move to become a critical hit</summary>
/// <example>23.2</example>
/// </param>
public record CreateMove
(
    string Name,
    string Description,
    float Damage,
    float CriticalHitFactor,
    float CriticalHitChance
);

/// <summary>
/// Contract for creating a new Stat
/// </summary>
/// <param name="Name">
/// <summary>The name of the stat</summary>
/// <example>Strength</example>
/// </param>
/// <param name="BaseStat">
/// <summary>The base stat value</summary>
/// <example>15</example>
/// </param>
/// <param name="Effort">
/// <summary>The effor needed for increasing the stat</summary>
/// <example>30</example>
/// </param>
public record CreateStat
(
    string Name,
    int BaseStat,
    int Effort
);

/// <summary>
/// Contract for creating a new Ability
/// </summary>
/// <param name="Name">
/// <summary>The name of the ability</summary>
/// <example>Electric Shield</example>
/// </param>
public record CreateAbility
(
    string Name
);

/// <summary>
/// Contract for creating a new Evolution
/// </summary>
/// <param name="Name">
/// <summary>The name of a evolution of the Pokemon</summary>
/// <example>Raichu</example>
/// </param>
public record CreateEvolution
(
    string Name
);