using System.Net;
using System.Net.Http.Json;
using PokemonCenter.Api.Dtos.Pokemon;
using PokemonCenter.IntegrationTests.Infrastructure;
using Xunit;

namespace PokemonCenter.IntegrationTests.Controllers
{
    public class PokemonDetailsEndpointTests : IClassFixture<PokemonCenterApiFactory>
    {
        private readonly HttpClient _client;

        public PokemonDetailsEndpointTests(PokemonCenterApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData("1")]
        [InlineData("bulbasaur")]
        public async Task GetPokemonDetails_WithExistingPokemon_ShouldReturnOk(string idOrName)
        {
            var url = string.Format("/api/pokemon/{0}", idOrName);

            var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var body = await response.Content.ReadFromJsonAsync<PokemonDetailsDto>();
            Assert.NotNull(body);

            Assert.Equal("bulbasaur", body!.Name.ToLowerInvariant());
            Assert.True(body.Id > 0);
            Assert.NotEmpty(body.Types);
            Assert.NotEmpty(body.Stats);
            Assert.NotNull(body.Sprites);
        }

        [Fact]
        public async Task GetPokemonDetails_WithUnknownPokemon_ShouldReturnNotFound()
        {
            const string idOrName = "missingno";
            var url = string.Format("/api/pokemon/{0}", idOrName);

            var factory = PokemonCenterApiFactory.CreateWithMode(PokeApiSimulationMode.NotFound);
            var client = factory.CreateClient();

            var response = await client.GetAsync(url);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetPokemonDetails_WhenPokeApiReturnsInvalidJson_ShouldReturnBadGateway()
        {
            const string idOrName = "bulbasaur";
            var url = string.Format("/api/pokemon/{0}", idOrName);

            var factory = PokemonCenterApiFactory.CreateWithMode(PokeApiSimulationMode.InvalidJson);
            var client = factory.CreateClient();

            var response = await client.GetAsync(url);

            Assert.Equal(HttpStatusCode.BadGateway, response.StatusCode);
        }

        [Fact]
        public async Task GetPokemonDetails_WhenPokeApiFails_ShouldReturnServiceUnavailable()
        {
            const string idOrName = "bulbasaur";
            var url = string.Format("/api/pokemon/{0}", idOrName);

            var factory = PokemonCenterApiFactory.CreateWithMode(PokeApiSimulationMode.HttpError);
            var client = factory.CreateClient();

            var response = await client.GetAsync(url);

            Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        }
    }
}
