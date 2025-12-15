using Xunit;
using PokemonCenter.Api.Dtos.Pokemon;
using PokemonCenter.Api.Services.Battle.Mappers;
using PokemonCenter.Api.Services.Battle.Models;

namespace PokemonCenter.UnitTests.Services.Battle.Mappers
{

    public class BattleResultMapperTests
    {
        [Fact]
        public void MapResultToDto_MapsWinnerLoserIdsNamesAndTurns()
        {
            var result = new BattleResult
            {
                Winner = "pikachu",
                WinnerHp = 10,
                Loser = "bulbasaur",
                LoserHp = 0,
                TotalTurns = 3,
                Turns = Array.Empty<BattleTurnLog>()
            };

            var participants = new[]
            {
            new PokemonDetailsDto { Id = 25, Name = "pikachu" },
            new PokemonDetailsDto { Id = 1, Name = "bulbasaur" }
        };

            var dto = BattleResultMapper.MapResultToDto(result, participants);

            Assert.NotNull(dto);

            Assert.NotNull(dto.Winner);
            Assert.Equal(25, dto.Winner.Id);
            Assert.Equal("pikachu", dto.Winner.Name);

            Assert.NotNull(dto.Loser);
            Assert.Equal(1, dto.Loser.Id);
            Assert.Equal("bulbasaur", dto.Loser.Name);

            Assert.Equal(3, dto.TotalTurns);
            Assert.NotNull(dto.Turns);
            Assert.Empty(dto.Turns);
        }
    }
}
