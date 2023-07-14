using ErrorOr;
using MediatR;
using Pokemon.Domain.PocketMonsterAggregate;

namespace Pokemon.Application.Pokemon.v1.Queries.GetPokemonById;

public record GetPokemonByIdQuery(string PokemonId) : IRequest<ErrorOr<PocketMonster>>;