using Xunit;
using PokemonCenter.Api.Dtos.Pokemon;
using PokemonCenter.Api.Dtos.pokemon;
using PokemonCenter.Api.Services.Battle.Mappers;

namespace PokemonCenter.UnitTests.Services.Battle.Mappers
{
    public class BattlePokemonMapperTests
    {
        [Fact]
        public void MapToBattlePokemon_MapsNameAndStats()
        {
            var details = new PokemonDetailsDto
            {
                Name = "pikachu",
                Stats = new()
            {
                new PokemonStatDto { Name = "hp", BaseValue = 35 },
                new PokemonStatDto { Name = "attack", BaseValue = 55 },
                new PokemonStatDto { Name = "defense", BaseValue = 40 },
                new PokemonStatDto { Name = "speed", BaseValue = 90 }
            }
            };

            var battlePokemon = BattlePokemonMapper.MapToBattlePokemon(details);

            Assert.Equal("pikachu", battlePokemon.Name);
            Assert.Equal(35, battlePokemon.Hp);
            Assert.Equal(55, battlePokemon.Attack);
            Assert.Equal(40, battlePokemon.Defense);
            Assert.Equal(90, battlePokemon.Speed);
        }
    }
}
