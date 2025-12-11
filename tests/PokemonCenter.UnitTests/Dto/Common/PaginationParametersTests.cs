using PokemonCenter.Api.Dtos.common;
using Xunit;

namespace PokemonCenter.UnitTests.Dto.Common
{
    public class PaginationParametersTests
    {
        [Fact]
        public void WhenNewPaginationParameters_WithValidArguments_ShouldSetPropertiesCorrectly()
        {
            var page = 2;
            var pageSize = 20;

            var pagination = new PaginationParameters(page, pageSize);

            Assert.Equal(page, pagination.Page);
            Assert.Equal(pageSize, pagination.PageSize);
            Assert.Equal(20, pagination.Offset);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public void WhenNewPaginationParameters_WithInvalidPage_ShouldThrowArgumentOutOfRangeException(int invalidPage)
        {
            var pageSize = 20;

            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => new PaginationParameters(invalidPage, pageSize));

            Assert.Equal("page", ex.ParamName);
            Assert.Contains("Page should be equal ou greater than 1", ex.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(101)]
        [InlineData(1000)]
        public void WhenNewPaginationParameters_WithInvalidPageSize_ShouldThrowArgumentOutOfRangeException(int invalidPageSize)
        {
            var page = 1;

            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => new PaginationParameters(page, invalidPageSize));

            Assert.Equal("pageSize", ex.ParamName);
            Assert.Contains("Page size should be between 1 and 100", ex.Message);
        }
    }
}
