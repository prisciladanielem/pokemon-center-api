using Microsoft.AspNetCore.Mvc;
using PokemonCenter.Api.Dtos;
using PokemonCenter.Api.Dtos.common;
using PokemonCenter.Api.Services.Interfaces;
using System.Text.Json;

namespace PokemonCenter.Api.Controllers
{
    [ApiController]
    [Route("api/pokemon")]
    public class PokedexController : ControllerBase
    {
        private readonly IPokedexService _pokemonService;

        public PokedexController(IPokedexService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<PokemonItemsDto>>> GetAllAsync(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = default)
        {
            if (page <= 0)
                return BadRequest("Page and pageSize must be greater than zero");

            if (pageSize < 1 || pageSize > 100)
                return BadRequest("Page size should be between 1 and 100");

            var result = await _pokemonService.GetPokemonListAsync(page, pageSize, cancellationToken);

            return Ok(result);
        }

        [HttpGet("{idOrName}")]
        public async Task<IActionResult> GetByIdOrName(string idOrName)
        {
            if (string.IsNullOrWhiteSpace(idOrName))
                return BadRequest("You must specify a pokémon id or name.");

            try
            {
                var pokemonDetails = await _pokemonService.GetPokemonDetailsAsync(idOrName);

                if (pokemonDetails is null)
                    return NotFound("Pokémon not found.");

                return Ok(pokemonDetails);
            }
            catch (JsonException)
            {
                return StatusCode(StatusCodes.Status502BadGateway, "Invalid response from external data provider.");
            }
            catch (HttpRequestException)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "External data provider is unavailable.");
            }
        }
    }
}
