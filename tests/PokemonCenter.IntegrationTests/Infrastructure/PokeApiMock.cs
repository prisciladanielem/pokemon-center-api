using System.Net;
using System.Text;

namespace PokemonCenter.IntegrationTests.Infrastructure
{

    using System.Net;
    using System.Text;

    public class PokeApiMock : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var path = request.RequestUri!.AbsolutePath.ToLowerInvariant();

            if (path.Contains("/pokemon/"))
            {
                var identifier = path.Split("/pokemon/")[1].Trim('/');
                var detailsJson = identifier switch
                {
                    "1" or "bulbasaur" => BulbasaurJson,
                    "25" or "pikachu" => PikachuJson,
                    _ => null
                };

                if (detailsJson is null)
                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));

                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(detailsJson, Encoding.UTF8, "application/json")
                });
            }

            if (path.EndsWith("/pokemon"))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(ListJson, Encoding.UTF8, "application/json")
                });
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        }

        private const string BulbasaurJson = @"
            {
            ""id"": 1,
            ""name"": ""bulbasaur"",
            ""height"": 7,
            ""weight"": 69,
            ""types"": [
                { ""slot"": 1, ""type"": { ""name"": ""grass"", ""url"": ""https://pokeapi.co/api/v2/type/12/"" } }
            ],
            ""abilities"": [
                { ""is_hidden"": false, ""slot"": 1, ""ability"": { ""name"": ""overgrow"", ""url"": ""https://pokeapi.co/api/v2/ability/65/"" } }
            ],
            ""stats"": [
                { ""base_stat"": 45, ""effort"": 0, ""stat"": { ""name"": ""hp"", ""url"": ""https://pokeapi.co/api/v2/stat/1/"" } },
                { ""base_stat"": 49, ""effort"": 0, ""stat"": { ""name"": ""attack"", ""url"": ""https://pokeapi.co/api/v2/stat/2/"" } },
                { ""base_stat"": 49, ""effort"": 0, ""stat"": { ""name"": ""defense"", ""url"": ""https://pokeapi.co/api/v2/stat/3/"" } },
                { ""base_stat"": 45, ""effort"": 0, ""stat"": { ""name"": ""speed"", ""url"": ""https://pokeapi.co/api/v2/stat/6/"" } }
            ],
            ""sprites"": {
                ""front_default"": ""front.png"",
                ""back_default"": ""back.png"",
                ""front_shiny"": ""front_shiny.png"",
                ""back_shiny"": ""back_shiny.png"",
                ""other"": { ""official-artwork"": { ""front_default"": ""art.png"" } }
            }
        }";

        private const string PikachuJson = @"
{
  ""id"": 25,
  ""name"": ""pikachu"",
  ""height"": 4,
  ""weight"": 60,
  ""types"": [
    { ""slot"": 1, ""type"": { ""name"": ""electric"", ""url"": ""https://pokeapi.co/api/v2/type/13/"" } }
  ],
  ""abilities"": [
    { ""is_hidden"": false, ""slot"": 1, ""ability"": { ""name"": ""static"", ""url"": ""https://pokeapi.co/api/v2/ability/9/"" } }
  ],
  ""stats"": [
    { ""base_stat"": 35, ""effort"": 0, ""stat"": { ""name"": ""hp"", ""url"": ""https://pokeapi.co/api/v2/stat/1/"" } },
    { ""base_stat"": 55, ""effort"": 0, ""stat"": { ""name"": ""attack"", ""url"": ""https://pokeapi.co/api/v2/stat/2/"" } },
    { ""base_stat"": 40, ""effort"": 0, ""stat"": { ""name"": ""defense"", ""url"": ""https://pokeapi.co/api/v2/stat/3/"" } },
    { ""base_stat"": 90, ""effort"": 0, ""stat"": { ""name"": ""speed"", ""url"": ""https://pokeapi.co/api/v2/stat/6/"" } }
  ],
  ""sprites"": {
    ""front_default"": ""front.png"",
    ""back_default"": ""back.png"",
    ""front_shiny"": ""front_shiny.png"",
    ""back_shiny"": ""back_shiny.png"",
    ""other"": { ""official-artwork"": { ""front_default"": ""art.png"" } }
  }
}";

        private const string ListJson = @"
{
  ""count"": 1302,
  ""results"": [
    { ""name"": ""bulbasaur"", ""url"": ""https://pokeapi.co/api/v2/pokemon/1/"" },
    { ""name"": ""pikachu"",   ""url"": ""https://pokeapi.co/api/v2/pokemon/25/"" }
  ]
}";
    }



    public class ErrorPokeApiMock : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            return Task.FromResult(response);
        }
    }

    public class InvalidJsonPokeApiMock : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            const string invalidJson = "{ invalid json";

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(invalidJson, Encoding.UTF8, "application/json")
            };

            return Task.FromResult(response);
        }
    }

    public class NotFoundPokeApiHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.NotFound);
            return Task.FromResult(response);
        }
    }
}
