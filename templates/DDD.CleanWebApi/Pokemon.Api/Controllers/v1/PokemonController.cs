using System.Security.Claims;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pokemon.Application.Pokemon.v1.Commands.CreatePokemon;
using Pokemon.Application.Pokemon.v1.Queries.GetPokemonById;
using Pokemon.Contracts.v1.Pokemon;

namespace Pokemon.Api.Controllers.v1;


[Route("api/v{version:apiVersion}/pokemon")]
[ApiVersion("1.0")]
[Produces(contentType: "application/json")]
public class PokemonController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public PokemonController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePokemon(CreatePokemonRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var command = _mapper.Map<CreatePokemonCommand>((request, userId));

        var createPokemonResult = await _mediator.Send(command);

        return createPokemonResult.Match(
            pokemon => Ok(_mapper.Map<PokemonResponse>(pokemon)),
            errors => Problem(errors));
    }

    [HttpGet]
    [Route("{pokemonId}")]
    public async Task<IActionResult> GetPokemonById(string pokemonId)
    {
        var query = _mapper.Map<GetPokemonByIdQuery>(pokemonId);

        var getPokemonResult = await _mediator.Send(query);

        return getPokemonResult.Match(
            pokemon => Ok(_mapper.Map<PokemonResponse>(pokemon)),
            errors => Problem(errors));
    }
}
