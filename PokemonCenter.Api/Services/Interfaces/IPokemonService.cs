using PokemonCenter.Api.Dtos;
using PokemonCenter.Api.Dtos.common;

namespace PokemonCenter.Api.Services.Interfaces
{
    public interface IPokemonService
    {
        Task<PagedResult<PokemonItemsDto>> GetPokemonListAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    }
}
