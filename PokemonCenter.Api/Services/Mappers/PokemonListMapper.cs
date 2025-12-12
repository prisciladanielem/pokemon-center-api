using PokemonCenter.Api.Dtos;
using PokemonCenter.Api.Dtos.common;
using PokemonCenter.Api.Infrastructure.External.PokeApi;
using PokemonCenter.Api.Services.Helpers;

namespace PokemonCenter.Api.Services.Mappers
{
    public static class PokemonListMapper
    {

        private static string BuildSpriteUrl(int id)
        {
            return string.Format(
                "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{0}.png",
                id
            );
        }

        private static PokemonItemsDto MapToListItemDto(PokeApiItem item)
        {
            var id = PokemonUrlParser.ExtractIdFromUrl(item.Url);

            return new PokemonItemsDto
            {
                Id = id,
                Name = item.Name,
                SpriteUrl = BuildSpriteUrl(id)
            };
        }

        public static List<PokemonItemsDto> MapToListItems(PokeApiResponse response)
        {
            return response.Results
                .Select(MapToListItemDto)
                .ToList();
        }

        public static PagedResult<PokemonItemsDto> CreatePagedResult(PaginationParameters pagination, int totalCount, IReadOnlyCollection<PokemonItemsDto> items)
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
