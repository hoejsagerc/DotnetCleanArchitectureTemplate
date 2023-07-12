using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Pokemon.Application.Common.Interfaces.Persistence;
using Pokemon.Domain.Common.PokemonAggregate.Entities;
using Pokemon.Domain.PocketMonsterAggregate;
using Pokemon.Domain.AuthenticationAggregates.ValueObjects;

namespace Pokemon.Application.Pokemon.v1.Commands.CreatePokemon;

public class CreatePocketMonsterCommandHandler
    : IRequestHandler<CreatePocketMonsterCommand, ErrorOr<PocketMonster>>
{
    private readonly IPocketMonsterRepository _pocketMonsterRepository;
    private readonly ILogger<CreatePocketMonsterCommandHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreatePocketMonsterCommandHandler(
        IPocketMonsterRepository pocketMonsterRepository,
        ILogger<CreatePocketMonsterCommandHandler> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _pocketMonsterRepository = pocketMonsterRepository;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ErrorOr<PocketMonster>> Handle(
        CreatePocketMonsterCommand command,
        CancellationToken cancellationToken)
    {
        string? sourceIpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

        var pokemon = PocketMonster.Create(
            name: command.Name,
            type: command.Type,
            height: command.Height,
            weight: command.Weight,
            level: command.Level,
            experience: command.Experience,
            experienceToNextLevel: command.ExperienceToNextLevel,
            sprite: command.Sprite,
            userId: UserId.Create(Guid.Parse(command.UserId)),
            moves: command.Moves.ConvertAll(move => Move.Create(
                name: move.Name,
                description: move.Description,
                damage: move.Damage,
                criticalHitFactor: move.CriticalHitFactor,
                criticalHitChance: move.CriticalHitChance
            )),
            stats: command.Stats.ConvertAll(stat => Stat.Create(
                name: stat.Name,
                baseStat: stat.BaseStat,
                effort: stat.Effort
            )),
            abilities: command.Abilities.ConvertAll(ability => Ability.Create(
                name: ability.Name
            )),
            evolutions: command.Evolutions.ConvertAll(evolution => Evolutions.Create(
                name: evolution.Name
            )));

        await _pocketMonsterRepository.AddAsync(pokemon);

        _logger.LogInformation("Pokemon created successful, {@Pokemon}, {@UserId}, {@SourceIpAddress}, {@DateTimeUtc}",
            pokemon, command.UserId, sourceIpAddress, DateTime.UtcNow);

        return pokemon;
    }
}