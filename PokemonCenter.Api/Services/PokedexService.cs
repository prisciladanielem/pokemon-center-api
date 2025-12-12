using PokemonCenter.Api.Dtos;
using PokemonCenter.Api.Dtos.common;
using PokemonCenter.Api.Dtos.Pokemon;
using PokemonCenter.Api.Infrastructure.External.PokeApi;
using PokemonCenter.Api.Services.Helpers;
using PokemonCenter.Api.Services.Interfaces;
using PokemonCenter.Api.Services.Mappers;
using System.Net;

namespace PokemonCenter.Api.Services
{
    public class PokedexService : IPokedexService
    {
        private readonly HttpClient _httpClient;

        public PokedexService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PagedResult<PokemonItemsDto>> GetPokemonListAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var pagination = new PaginationParameters(page, pageSize);

            var uri = PokedexUriBuilder.BuildPokemonListUri(pagination);

            var pokeApiResponse = await GetPokeApiListResponseAsync(uri, cancellationToken);

            var items = PokemonListMapper.MapToListItems(pokeApiResponse);

            return PokemonListMapper.CreatePagedResult(pagination, pokeApiResponse.Count, items);
        }

        public async Task<PokemonDetailsDto?> GetPokemonDetailsAsync(string idOrName)
        {
            var normalized = idOrName.Trim().ToLowerInvariant();
            var url = string.Format("pokemon/{0}", normalized);

            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            var pokemonDetails = await response.Content.ReadFromJsonAsync<PokeApiPokemonDetails>();

            return PokemonDetailsMapper.MapToPokemonDetailsDto(pokemonDetails);
        }

        private async Task<PokeApiResponse> GetPokeApiListResponseAsync(string uri, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync(uri, cancellationToken);

            response.EnsureSuccessStatusCode();

            var pokeApiResponse = await response.Content.ReadFromJsonAsync<PokeApiResponse>() ?? new PokeApiResponse();

            return pokeApiResponse;
        }
    }
}
