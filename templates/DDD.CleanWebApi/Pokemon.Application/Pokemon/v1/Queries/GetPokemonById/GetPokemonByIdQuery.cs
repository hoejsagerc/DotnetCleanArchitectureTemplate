using ErrorOr;
using MediatR;
using Pokemon.Domain.PokemonAggregate;

namespace Pokemon.Application.Pokemon.v1.Queries.GetPokemonById;

public record GetPokemonByIdQuery(string PokemonId) : IRequest<ErrorOr<PocketMonster>>;