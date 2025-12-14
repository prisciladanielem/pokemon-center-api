using PokemonCenter.Api.Dtos;
using PokemonCenter.Api.Dtos.common;
using PokemonCenter.Api.Dtos.Pokemon;

namespace PokemonCenter.Api.Services.Pokedex.Interfaces
{
    public interface IPokedexService
    {
        Task<PagedResult<PokemonItemsDto>> GetPokemonListAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<PokemonDetailsDto?> GetPokemonDetailsAsync(string idOrName);
    }
}
