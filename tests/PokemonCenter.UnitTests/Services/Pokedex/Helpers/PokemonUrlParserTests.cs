using PokemonCenter.Api.Services.Pokedex.Helpers;
using Xunit;

namespace PokemonCenter.UnitTests.Services.Pokedex.Helpers
{
    public class PokemonUrlParserTests
    {
        [Fact]
        public void ExtractIdFromUrl_WithTrailingSlash_ReturnsExpectedId()
        {
            var url = "https://pokeapi.co/api/v2/pokemon/25/";

            var id = PokemonUrlParser.ExtractIdFromUrl(url);

            Assert.Equal(25, id);
        }

        [Fact]
        public void ExtractIdFromUrl_WithoutTrailingSlash_ReturnsExpectedId()
        {
            var url = "https://pokeapi.co/api/v2/pokemon/150";

            var id = PokemonUrlParser.ExtractIdFromUrl(url);

            Assert.Equal(150, id);
        }

        [Fact]
        public void ExtractIdFromUrl_WithOnlyIdSegment_ReturnsExpectedId()
        {
            var url = "133";

            var id = PokemonUrlParser.ExtractIdFromUrl(url);

            Assert.Equal(133, id);
        }

        [Fact]
        public void ExtractIdFromUrl_WithNonNumericLastSegment_ReturnsZero()
        {
            var url = "https://pokeapi.co/api/v2/pokemon/pikachu/";

            var id = PokemonUrlParser.ExtractIdFromUrl(url);

            Assert.Equal(0, id);
        }
    }
}
