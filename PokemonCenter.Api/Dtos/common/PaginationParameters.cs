namespace PokemonCenter.Api.Dtos.common
{
    public class PaginationParameters
    {
        public int Page { get; }
        public int PageSize { get; }

        public PaginationParameters (int page, int pageSize)
        {
            if (page < 1)
                throw new ArgumentOutOfRangeException(nameof(page), "Page should be equal ou greater than 1");

            if (pageSize < 1 || pageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size should be between 1 and 100");

            Page = page;
            PageSize = pageSize;

        }

        public int Offset => (Page - 1) * PageSize;
    }
}