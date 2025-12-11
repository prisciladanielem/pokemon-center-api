namespace PokemonCenter.Api.Dtos.common
{
    //I´m using generic here, because if we decided to extend our project to do another get all requests, we are able to reuse this class to paginate
    public class PagedResult<T> 
    {
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int TotalCount { get; init; }
        public int TotalPages { get; init; }
        public IReadOnlyCollection<T> Items { get; init; } = Array.Empty<T>();
    }
}