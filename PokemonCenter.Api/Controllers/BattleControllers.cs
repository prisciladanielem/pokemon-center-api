using Microsoft.AspNetCore.Mvc;
using PokemonCenter.Api.Dtos.Battle;
using PokemonCenter.Api.Services.Battle.Exceptions;
using PokemonCenter.Api.Services.Battle.Interfaces;

namespace PokemonCenter.Api.Controllers;

[ApiController]
[Route("api/battle")]
public class BattleController : ControllerBase
{
    private readonly IBattleSimulationService _battleSimulationService;

    public BattleController(IBattleSimulationService battleSimulationService)
    {
        _battleSimulationService = battleSimulationService;
    }

    [HttpPost("simulate")]
    public async Task<IActionResult> Simulate([FromBody] BattleRequestDto request)
    {
        if (request is null)
        {
            return BadRequest("Request body is required");
        }

        if (string.IsNullOrWhiteSpace(request.FirstPokemon) || string.IsNullOrWhiteSpace(request.SecondPokemon))
        {
            return BadRequest("firstPokemon and secondPokemon must be provided");
        }

        if (string.Equals(request.FirstPokemon, request.SecondPokemon, StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("Pokémon must be different to battle");
        }

        try
        {
            var result = await _battleSimulationService.SimulateAsync(request);
            
            return Ok(result);
        }
        catch (PokemonNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
