using Microsoft.AspNetCore.Mvc;
using PokemonCenter.Api.Dtos;
using PokemonCenter.Api.Dtos.common;
using PokemonCenter.Api.Services.Interfaces;
using System.Text.Json;

namespace PokemonCenter.Api.Controllers
{
    [ApiController]
    [Route("api/pokemon")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;

        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<PokemonItemsDto>>> GetAllAsync(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _pokemonService.GetPokemonListAsync(page, pageSize, cancellationToken);
                return Ok(result);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (HttpRequestException)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new { error = "Failed to retrieve data from external pokémon service" });
            }
            catch (JsonException)
            {
                return StatusCode(StatusCodes.Status502BadGateway, new { error = "Received an invalid response format from external pokémon service" });
            }
        }
    }
}
