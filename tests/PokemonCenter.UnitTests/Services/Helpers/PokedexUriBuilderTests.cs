using PokemonCenter.Api.Dtos.common;
using PokemonCenter.Api.Services.Helpers;
using Xunit;

namespace PokemonCenter.UnitTests.Services.Helpers
{
    public class PokedexUriBuilderTests
    {
        [Fact]
        public void BuildPokemonListUri_Page1_ReturnsExpectedQueryString()
        {
            var pagination = new PaginationParameters(page: 1, pageSize: 20);

            var uri = PokedexUriBuilder.BuildPokemonListUri(pagination);

            Assert.Equal("pokemon?limit=20&offset=0", uri);
        }

        [Fact]
        public void BuildPokemonListUri_Page3_ReturnsExpectedQueryString()
        {
            var pagination = new PaginationParameters(page: 3, pageSize: 25);

            var uri = PokedexUriBuilder.BuildPokemonListUri(pagination);

            Assert.Equal("pokemon?limit=25&offset=50", uri);
        }

        [Fact]
        public void BuildPokemonListUri_UsesPaginationPropertiesDirectly()
        {
            var pagination = new PaginationParameters(page: 5, pageSize: 10);

            var uri = PokedexUriBuilder.BuildPokemonListUri(pagination);

            Assert.Equal("pokemon?limit=10&offset=40", uri);
        }
    }
}
