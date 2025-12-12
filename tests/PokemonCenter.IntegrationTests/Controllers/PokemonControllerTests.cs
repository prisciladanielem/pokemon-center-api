using System.Net;
using System.Net.Http.Json;
using PokemonCenter.Api.Dtos;
using PokemonCenter.Api.Dtos.common;
using PokemonCenter.IntegrationTests.Infrastructure;
using Xunit;

namespace PokemonCenter.IntegrationTests.Controllers
{
    public class PokemonControllerTests : IClassFixture<PokemonCenterApiFactory>
    {
        private readonly HttpClient _client;

        public PokemonControllerTests(PokemonCenterApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GeAllPokemon_ShouldReturnOkAndValidPagedResult()
        {
            int page = 1;
            int pageSize = 5;
            var url = string.Format("/api/pokemon?page={0}&pageSize={1}", page, pageSize);

            var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<PagedResult<PokemonItemsDto>>();

            Assert.NotNull(content);
            Assert.Equal(page, content!.Page);
            Assert.Equal(pageSize, content.PageSize);
            Assert.True(content.TotalCount > 0);
            Assert.NotNull(content.Items);
            Assert.True(content.Items.Count > 0);


            var first = content.Items.First();
            Assert.True(first.Id > 0);
            Assert.False(string.IsNullOrWhiteSpace(first.Name));
            Assert.False(string.IsNullOrWhiteSpace(first.SpriteUrl));
        }

        [Fact]
        public async Task GetAllPokemon_WithInvalidPage_ShouldReturnBadRequest()
        {
            const int page = 0;
            const int pageSize = 10;
            var url = string.Format("/api/pokemon?page={0}&pageSize={1}", page, pageSize);

            var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var errorBody = await response.Content.ReadAsStringAsync();
            Assert.Contains("Page should be equal ou greater than 1", errorBody);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        [InlineData(101)]
        public async Task GetAllPokemon_WithInvalidPageSize_ShouldReturnBadRequest(int invalidPageSize)
        {
            const int page = 1;
            var url = string.Format("/api/pokemon?page={0}&pageSize={1}", page, invalidPageSize);

            var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var errorBody = await response.Content.ReadAsStringAsync();
            Assert.Contains("Page size should be between 1 and 100", errorBody);
        }

        [Fact]
        public async Task GetAllPokemon_WhenExternalServiceFails_ShouldReturnServiceUnavailable()
        {
            await using var factory = PokemonCenterApiFactory.CreateWithMode(PokeApiSimulationMode.HttpError);
            using var client = factory.CreateClient();

            var url = "/api/pokemon?page=1&pageSize=5";

            var response = await client.GetAsync(url);

            Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);

            var errorBody = await response.Content.ReadAsStringAsync();
            Assert.Contains("Failed to retrieve data from external pokémon service", errorBody);
        }

        [Fact]
        public async Task GeAlltPokemon_WhenExternalServiceReturnsInvalidJson_ShouldReturnBadGateway()
        {
            await using var factory = PokemonCenterApiFactory.CreateWithMode(PokeApiSimulationMode.InvalidJson);
            using var client = factory.CreateClient();

            var url = "/api/pokemon?page=1&pageSize=5";

            var response = await client.GetAsync(url);

            Assert.Equal(HttpStatusCode.BadGateway, response.StatusCode);

            var errorBody = await response.Content.ReadAsStringAsync();
            Assert.Contains("Received an invalid response format from external pokémon service", errorBody);
        }

    }
}
