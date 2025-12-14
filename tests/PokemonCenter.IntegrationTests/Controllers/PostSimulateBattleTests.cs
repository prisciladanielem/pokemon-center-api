using System.Net;
using System.Net.Http.Json;
using Xunit;
using PokemonCenter.Api.Dtos.Battle;
using PokemonCenter.IntegrationTests.Infrastructure;
using System.Text;

namespace PokemonCenter.IntegrationTests.Controllers
{
    public class PostSimulateBattleTests
    {
        [Fact]
        public async Task Post_Simulate_WhenSuccess_Returns200_And_ResponseRespectsInvariants()
        {
            var factory = PokemonCenterApiFactory.CreateWithMode(PokeApiSimulationMode.Success);
            var client = factory.CreateClient();

            var request = new BattleRequestDto
            {
                FirstPokemon = "pikachu",
                SecondPokemon = "bulbasaur"
            };

            var response = await client.PostAsJsonAsync("/api/battle/simulate", request);

            var bodyText = await response.Content.ReadAsStringAsync();
            Assert.True(response.IsSuccessStatusCode, $"Status: {response.StatusCode}\nBody: {bodyText}");

            var result = await response.Content.ReadFromJsonAsync<BattleResultDto>();
            Assert.NotNull(result);

            Assert.NotNull(result!.Winner);
            Assert.NotNull(result.Loser);

            Assert.False(string.IsNullOrWhiteSpace(result.Winner.Name));
            Assert.False(string.IsNullOrWhiteSpace(result.Loser.Name));

            Assert.NotEqual(result.Winner.Name, result.Loser.Name);

            Assert.True(result.Winner.Id > 0);
            Assert.True(result.Loser.Id > 0);

            Assert.True(result.TotalTurns > 0);

            Assert.NotNull(result.Turns);
            Assert.NotEmpty(result.Turns);

            Assert.Equal(result.TotalTurns, result.Turns.Count);
        }

        [Fact]
        public async Task Post_Simulate_WhenPokemonsAreEqual_Returns400()
        {
            var factory = PokemonCenterApiFactory.CreateWithMode(PokeApiSimulationMode.Success);
            var client = factory.CreateClient();

            var request = new BattleRequestDto
            {
                FirstPokemon = "pikachu",
                SecondPokemon = "Pikachu"
            };

            var response = await client.PostAsJsonAsync("/api/battle/simulate", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrWhiteSpace(body));
        }

        [Fact]
        public async Task Post_Simulate_WhenMissingFields_Returns400()
        {
            var factory = PokemonCenterApiFactory.CreateWithMode(PokeApiSimulationMode.Success);
            var client = factory.CreateClient();

            var request = new BattleRequestDto
            {
                FirstPokemon = "",
                SecondPokemon = "bulbasaur"
            };

            var response = await client.PostAsJsonAsync("/api/battle/simulate", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrWhiteSpace(body));
        }

        [Fact]
        public async Task Post_Simulate_WhenPokemonNotFound_Returns404()
        {
            var factory = PokemonCenterApiFactory.CreateWithMode(PokeApiSimulationMode.NotFound);
            var client = factory.CreateClient();

            var request = new BattleRequestDto
            {
                FirstPokemon = "missing",
                SecondPokemon = "bulbasaur"
            };

            var response = await client.PostAsJsonAsync("/api/battle/simulate", request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrWhiteSpace(body));
        }

        [Fact]
        public async Task Post_Simulate_WhenRequestBodyIsMissing_Returns400()
        {
            var factory = PokemonCenterApiFactory.CreateWithMode(PokeApiSimulationMode.Success);
            var client = factory.CreateClient();

            var response = await client.PostAsync("/api/battle/simulate", new StringContent("", Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrWhiteSpace(body));
        }
    }
}
