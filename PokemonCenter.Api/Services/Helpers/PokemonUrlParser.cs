namespace PokemonCenter.Api.Services.Helpers
{
    public static class PokemonUrlParser
    {
        public static int ExtractIdFromUrl(string url)
        {
            var lastSegment = url.TrimEnd('/').Split('/').Last();
            return int.TryParse(lastSegment, out var id) ? id : 0;
        }
    }
}
