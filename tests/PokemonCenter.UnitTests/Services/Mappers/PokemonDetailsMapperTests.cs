using PokemonCenter.Api.Infrastructure.External.PokeApi;
using PokemonCenter.Api.Services.Mappers;
using Xunit;

namespace PokemonCenter.UnitTests.Services.Mappers
{
    public class PokemonDetailsMapperTests
    {
        [Fact]
        public void MapToPokemonDetailsDto_MapsBasicPropertiesCorrectly()
        {
            var source = new PokeApiPokemonDetails
            {
                Id = 25,
                Name = "pikachu",
                Height = 4,
                Weight = 60,
                Sprites = new PokeApiPokemonSprites()
            };

            var result = PokemonDetailsMapper.MapToPokemonDetailsDto(source);

            Assert.Equal(25, result.Id);
            Assert.Equal("pikachu", result.Name);
            Assert.Equal(4, result.Height);
            Assert.Equal(60, result.Weight);
        }

        [Fact]
        public void MapToPokemonDetailsDto_OrdersTypesBySlotAndProjectsNames()
        {
            var source = new PokeApiPokemonDetails
            {
                Types = new List<PokeApiPokemonTypeSlot>
                {
                    new()
                    {
                        Slot = 2,
                        Type = new PokeApiNamedResource { Name = "flying" }
                    },
                    new()
                    {
                        Slot = 1,
                        Type = new PokeApiNamedResource { Name = "normal" }
                    }
                },
                Sprites = new PokeApiPokemonSprites()
            };

            var result = PokemonDetailsMapper.MapToPokemonDetailsDto(source);

            Assert.Equal(new[] { "normal", "flying" }, result.Types);
        }

        [Fact]
        public void MapToPokemonDetailsDto_OrdersAbilitiesBySlotAndProjectsNames()
        {
            var source = new PokeApiPokemonDetails
            {
                Abilities = new List<PokeApiPokemonAbilitySlot>
                {
                    new()
                    {
                        Slot = 3,
                        Ability = new PokeApiNamedResource { Name = "lightning-rod" }
                    },
                    new()
                    {
                        Slot = 1,
                        Ability = new PokeApiNamedResource { Name = "static" }
                    }
                },
                Sprites = new PokeApiPokemonSprites()
            };

            var result = PokemonDetailsMapper.MapToPokemonDetailsDto(source);

            Assert.Equal(new[] { "static", "lightning-rod" }, result.Abilities);
        }

        [Fact]
        public void MapToPokemonDetailsDto_MapsStatsNameAndBaseValue()
        {
            var source = new PokeApiPokemonDetails
            {
                Stats = new List<PokeApiPokemonStat>
                {
                    new()
                    {
                        BaseStat = 35,
                        Stat = new PokeApiNamedResource { Name = "hp" }
                    },
                    new()
                    {
                        BaseStat = 90,
                        Stat = new PokeApiNamedResource { Name = "speed" }
                    }
                },
                Sprites = new PokeApiPokemonSprites()
            };

            var result = PokemonDetailsMapper.MapToPokemonDetailsDto(source);

            Assert.Equal(2, result.Stats.Count);

            Assert.Contains(result.Stats, s =>
                s.Name == "hp" && s.BaseValue == 35);

            Assert.Contains(result.Stats, s =>
                s.Name == "speed" && s.BaseValue == 90);
        }

        [Fact]
        public void MapToPokemonDetailsDto_MapsSpritesIncludingOfficialArtwork()
        {
            var source = new PokeApiPokemonDetails
            {
                Sprites = new PokeApiPokemonSprites
                {
                    FrontDefault = "front.png",
                    BackDefault = "back.png",
                    FrontShiny = "front-shiny.png",
                    BackShiny = "back-shiny.png",
                    Other = new PokeApiPokemonOtherSprites
                    {
                        OfficialArtwork = new PokeApiPokemonOfficialArtwork
                        {
                            FrontDefault = "official.png"
                        }
                    }
                }
            };

            var result = PokemonDetailsMapper.MapToPokemonDetailsDto(source);

            Assert.Equal("front.png", result.Sprites.FrontDefault);
            Assert.Equal("back.png", result.Sprites.BackDefault);
            Assert.Equal("front-shiny.png", result.Sprites.FrontShiny);
            Assert.Equal("back-shiny.png", result.Sprites.BackShiny);
            Assert.Equal("official.png", result.Sprites.OfficialArtwork);
        }

        [Fact]
        public void MapToPokemonDetailsDto_AllowsNullOtherSprites()
        {
            var source = new PokeApiPokemonDetails
            {
                Sprites = new PokeApiPokemonSprites
                {
                    FrontDefault = "front.png",
                    Other = null
                }
            };

            var result = PokemonDetailsMapper.MapToPokemonDetailsDto(source);

            Assert.Equal("front.png", result.Sprites.FrontDefault);
            Assert.Null(result.Sprites.OfficialArtwork);
        }
    }
}
