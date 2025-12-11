using PokemonCenter.Api.Dtos;
using PokemonCenter.Api.Dtos.common;
using PokemonCenter.Api.Infrastructure.External.PokeApi;
using PokemonCenter.Api.Services.Interfaces;

namespace PokemonCenter.Api.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly HttpClient _httpClient;
        private const string PokemonListUrl = "pokemon?limit={0}&offset={1}";

        public PokemonService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PagedResult<PokemonItemsDto>> GetPokemonListAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var pagination = new PaginationParameters(page, pageSize);

            var uri = BuildPokemonListUri(pagination);

            var pokeApiResponse = await GetPokeApiListResponseAsync(uri, cancellationToken);

            var items = MapToListItems(pokeApiResponse);

            return CreatePagedResult(pagination, pokeApiResponse.Count, items);
        }

        private async Task<PokeApiResponse> GetPokeApiListResponseAsync(string uri, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync(uri, cancellationToken);

            response.EnsureSuccessStatusCode();

            var pokeApiResponse = await response.Content.ReadFromJsonAsync<PokeApiResponse>() ?? new PokeApiResponse();

            return pokeApiResponse;
        }

        private List<PokemonItemsDto> MapToListItems(PokeApiResponse response)
        {
            return response.Results
                .Select(MapToListItemDto)
                .ToList();
        }

        private PokemonItemsDto MapToListItemDto(PokeApiItem item)
        {
            var id = ExtractIdFromUrl(item.Url);

            return new PokemonItemsDto
            {
                Id = id,
                Name = item.Name,
                SpriteUrl = BuildSpriteUrl(id)
            };
        }

        private static string BuildPokemonListUri(PaginationParameters pagination)
        {
            return string.Format(
                PokemonListUrl,
                pagination.PageSize,
                pagination.Offset);
        }

        private static int ExtractIdFromUrl(string url)
        {
            var trimmed = url.TrimEnd('/');
            var last = trimmed.Split('/')[^1];
            return int.TryParse(last, out var id) ? id : 0;
        }

        private static string BuildSpriteUrl(int id)
        {
            return string.Format(
                "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{0}.png",
                id
            );
        }

        private static PagedResult<PokemonItemsDto> CreatePagedResult(PaginationParameters pagination, int totalCount, IReadOnlyCollection<PokemonItemsDto> items)
        {
            var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize);

            return new PagedResult<PokemonItemsDto>
            {
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Items = items
            };
        }

    }
}
