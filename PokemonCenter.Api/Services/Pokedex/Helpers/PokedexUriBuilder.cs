using PokemonCenter.Api.Dtos.common;

namespace PokemonCenter.Api.Services.Pokedex.Helpers
{
    public static class PokedexUriBuilder
    {
        private const string PokemonListUrl = "pokemon?limit={0}&offset={1}";

        public static string BuildPokemonListUri(PaginationParameters pagination)
        {
            return string.Format(
                PokemonListUrl,
                pagination.PageSize,
                pagination.Offset);
        }
    }
}
