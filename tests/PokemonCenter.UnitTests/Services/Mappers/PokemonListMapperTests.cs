using PokemonCenter.Api.Dtos;
using PokemonCenter.Api.Dtos.common;
using PokemonCenter.Api.Infrastructure.External.PokeApi;
using PokemonCenter.Api.Services.Mappers;
using Xunit;

namespace PokemonCenter.UnitTests.Services.Mappers
{
    public class PokemonListMapperTests
    {
        [Fact]
        public void MapToListItems_MapsCorrectly()
        {
            var response = new PokeApiResponse
            {
                Count = 2,
                Results = new List<PokeApiItem>
                {
                    new()
                    {
                        Name = "pikachu",
                        Url = "https://pokeapi.co/api/v2/pokemon/25/"
                    },
                    new()
                    {
                        Name = "bulbasaur",
                        Url = "https://pokeapi.co/api/v2/pokemon/1/"
                    }
                }
            };

            var items = PokemonListMapper.MapToListItems(response);

            Assert.Equal(2, items.Count);

            var pikachu = items.Single(x => x.Name == "pikachu");
            Assert.Equal(25, pikachu.Id);
            Assert.Equal("https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/25.png", pikachu.SpriteUrl);

            var bulbasaur = items.Single(x => x.Name == "bulbasaur");
            Assert.Equal(1, bulbasaur.Id);
            Assert.Equal("https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/1.png", bulbasaur.SpriteUrl);
        }

        [Fact]
        public void MapToListItems_ReturnsEmptyList_WhenNoResults()
        {
            var response = new PokeApiResponse
            {
                Count = 0,
                Results = new List<PokeApiItem>()
            };

            var items = PokemonListMapper.MapToListItems(response);

            Assert.Empty(items);
        }

        [Fact]
        public void CreatePagedResult_ComputesTotalPagesCorrectly()
        {
            var pagination = new PaginationParameters(page: 2, pageSize: 20);
            var totalCount = 45; 

            var items = new List<PokemonItemsDto>
            {
                new() { Id = 1, Name = "bulbasaur", SpriteUrl = "url1" },
                new() { Id = 2, Name = "ivysaur", SpriteUrl = "url2" }
            };

            var result = PokemonListMapper.CreatePagedResult(pagination, totalCount, items);

            Assert.Equal(2, result.Page);
            Assert.Equal(20, result.PageSize);
            Assert.Equal(45, result.TotalCount);
            Assert.Equal(3, result.TotalPages);
            Assert.Equal(items, result.Items);
        }
    }
}
