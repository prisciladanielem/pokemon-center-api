using Moq;
using Xunit;
using PokemonCenter.Api.Dtos.Battle;
using PokemonCenter.Api.Dtos.Pokemon;
using PokemonCenter.Api.Services.Battle;
using PokemonCenter.Api.Services.Battle.Exceptions;
using PokemonCenter.Api.Services.Battle.Interfaces;
using PokemonCenter.Api.Services.Battle.Models;
using PokemonCenter.Api.Services.Pokedex.Interfaces;
using PokemonCenter.Api.Dtos.pokemon;

namespace PokemonCenter.UnitTests.Services.Battle
{
    public class BattleSimulationServiceTests
    {
        private readonly Mock<IBattleService> _battleServiceMock = new(MockBehavior.Strict);
        private readonly Mock<IPokedexService> _pokedexServiceMock = new(MockBehavior.Strict);

        [Fact]
        public async Task SimulateAsync_WhenBothPokemonsExist_ReturnsBattleResultDto()
        {
            var request = new BattleRequestDto
            {
                FirstPokemon = "pikachu",
                SecondPokemon = "bulbasaur"
            };

            var pikachu = BuildPokemonDetails("pikachu");
            var bulbasaur = BuildPokemonDetails("bulbasaur");

            _pokedexServiceMock
                .Setup(x => x.GetPokemonDetailsAsync("pikachu"))
                .ReturnsAsync(pikachu);

            _pokedexServiceMock
                .Setup(x => x.GetPokemonDetailsAsync("bulbasaur"))
                .ReturnsAsync(bulbasaur);

            var battleResult = new BattleResult
            {
                Winner = "pikachu",
                WinnerHp = 10,
                Loser = "bulbasaur",
                LoserHp = 0,
                TotalTurns = 3,
                Turns = Array.Empty<BattleTurnLog>()
            };

            _battleServiceMock
                .Setup(x => x.Simulate(It.IsAny<BattlePokemon>(), It.IsAny<BattlePokemon>()))
                .Returns(battleResult);

            var service = CreateService();

            var result = await service.SimulateAsync(request);

            Assert.NotNull(result);

            _pokedexServiceMock.Verify(x => x.GetPokemonDetailsAsync("pikachu"),
                Times.Once);

            _pokedexServiceMock.Verify(x => x.GetPokemonDetailsAsync("bulbasaur"),
                Times.Once);

            _battleServiceMock.Verify(x => x.Simulate(It.IsAny<BattlePokemon>(), It.IsAny<BattlePokemon>()),
                Times.Once);

            _battleServiceMock.VerifyNoOtherCalls();
            _pokedexServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task SimulateAsync_WhenFirstPokemonNotFound_ThrowsPokemonNotFoundException()
        {
            var request = new BattleRequestDto
            {
                FirstPokemon = "missing",
                SecondPokemon = "bulbasaur"
            };

            _pokedexServiceMock
                .Setup(x => x.GetPokemonDetailsAsync("missing"))
                .ReturnsAsync((PokemonDetailsDto?)null);

            _pokedexServiceMock
                .Setup(x => x.GetPokemonDetailsAsync("bulbasaur"))
                .ReturnsAsync(BuildPokemonDetails("bulbasaur"));

            var service = CreateService();

            var ex = await Assert.ThrowsAsync<PokemonNotFoundException>(
                () => service.SimulateAsync(request));

            Assert.Contains("missing", ex.Message, StringComparison.OrdinalIgnoreCase);

            _battleServiceMock.Verify(x => x.Simulate(It.IsAny<BattlePokemon>(), It.IsAny<BattlePokemon>()),
                Times.Never);

            _pokedexServiceMock.Verify(x => x.GetPokemonDetailsAsync("missing"),
                 Times.Once);

            _pokedexServiceMock.Verify(x => x.GetPokemonDetailsAsync("bulbasaur"),
                Times.Once);

            _pokedexServiceMock.VerifyNoOtherCalls();
            _battleServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task SimulateAsync_WhenSecondPokemonNotFound_ThrowsPokemonNotFoundException()
        {
            var request = new BattleRequestDto
            {
                FirstPokemon = "pikachu",
                SecondPokemon = "missing"
            };

            _pokedexServiceMock
                .Setup(x => x.GetPokemonDetailsAsync("pikachu"))
                .ReturnsAsync(BuildPokemonDetails("pikachu"));

            _pokedexServiceMock
                .Setup(x => x.GetPokemonDetailsAsync("missing"))
                .ReturnsAsync((PokemonDetailsDto?)null);

            var service = CreateService();

            var ex = await Assert.ThrowsAsync<PokemonNotFoundException>(
                () => service.SimulateAsync(request));

            Assert.Contains("missing", ex.Message, StringComparison.OrdinalIgnoreCase);

            _battleServiceMock.Verify(x => x.Simulate(It.IsAny<BattlePokemon>(), It.IsAny<BattlePokemon>()),
                Times.Never);

            _pokedexServiceMock.Verify(x => x.GetPokemonDetailsAsync("missing"),
                Times.Once);

            _pokedexServiceMock.Verify(x => x.GetPokemonDetailsAsync("pikachu"),
                Times.Once);

            _pokedexServiceMock.VerifyNoOtherCalls();
            _battleServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task SimulateAsync_WhenPokedexServiceThrows_ExceptionIsPropagated()
        {
            var request = new BattleRequestDto
            {
                FirstPokemon = "pikachu",
                SecondPokemon = "bulbasaur"
            };

            _pokedexServiceMock
                .Setup(x => x.GetPokemonDetailsAsync("pikachu"))
                .ThrowsAsync(new HttpRequestException("boom"));

            _pokedexServiceMock
                .Setup(x => x.GetPokemonDetailsAsync("bulbasaur"))
                .ReturnsAsync(BuildPokemonDetails("bulbasaur"));

            var service = CreateService();

            await Assert.ThrowsAsync<HttpRequestException>(
                () => service.SimulateAsync(request));

            _battleServiceMock.Verify(x => x.Simulate(It.IsAny<BattlePokemon>(), It.IsAny<BattlePokemon>()),
                Times.Never);

            _pokedexServiceMock.Verify(x => x.GetPokemonDetailsAsync("pikachu"),
                Times.Once);

            _pokedexServiceMock.Verify(x => x.GetPokemonDetailsAsync("bulbasaur"),
                Times.Once);

            _pokedexServiceMock.VerifyNoOtherCalls();
            _battleServiceMock.VerifyNoOtherCalls();
        }

        private BattleSimulationService CreateService()
        {
            return new BattleSimulationService(_battleServiceMock.Object, _pokedexServiceMock.Object);
        }

        private static PokemonDetailsDto BuildPokemonDetails(string name)
        {
            return new PokemonDetailsDto
            {
                Id = 1,
                Name = name,
                Height = 10,
                Weight = 10,
                Types = new() { "electric" },
                Abilities = new() { "static" },
                Stats = new()
            {
                new PokemonStatDto { Name = "hp", BaseValue = 35 },
                new PokemonStatDto { Name = "attack", BaseValue = 55 },
                new PokemonStatDto { Name = "defense", BaseValue = 40 },
                new PokemonStatDto { Name = "speed", BaseValue = 90 }
            },
                Sprites = new()
            };
        }
    }

}