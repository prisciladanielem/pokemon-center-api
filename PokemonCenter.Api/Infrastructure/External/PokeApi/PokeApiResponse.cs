namespace PokemonCenter.Api.Infrastructure.External.PokeApi
{
    internal class PokeApiResponse
    {
        public int Count { get; set; }
        public List<PokeApiItem> Results { get; set; } = new();
    }
}
