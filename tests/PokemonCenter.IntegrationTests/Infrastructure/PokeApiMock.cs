using System.Net;
using System.Text;

namespace PokemonCenter.IntegrationTests.Infrastructure
{

    public class PokeApiMock : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var path = request.RequestUri!.AbsolutePath.ToLowerInvariant();

            if (path.Contains("/pokemon/"))
            {
                const string detailsJson = @"
            {
                ""id"": 1,
                ""name"": ""bulbasaur"",
                ""height"": 7,
                ""weight"": 69,
                ""types"": [
                    {
                        ""slot"": 1,
                        ""type"": { ""name"": ""grass"", ""url"": ""https://pokeapi.co/api/v2/type/12/"" }
                    }
                ],
                ""abilities"": [
                    {
                        ""is_hidden"": false,
                        ""slot"": 1,
                        ""ability"": { ""name"": ""overgrow"", ""url"": ""https://pokeapi.co/api/v2/ability/65/"" }
                    }
                ],
                ""stats"": [
                    {
                        ""base_stat"": 45,
                        ""effort"": 0,
                        ""stat"": { ""name"": ""speed"", ""url"": ""https://pokeapi.co/api/v2/stat/6/"" }
                    }
                ],
                ""sprites"": {
                    ""front_default"": ""front.png"",
                    ""back_default"": ""back.png"",
                    ""front_shiny"": ""front_shiny.png"",
                    ""back_shiny"": ""back_shiny.png"",
                    ""other"": {
                        ""official-artwork"": {
                            ""front_default"": ""art.png""
                        }
                    }
                }
            }";

                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(detailsJson, Encoding.UTF8, "application/json")
                });
            }

            if (path.EndsWith("/pokemon"))
            {
                const string listJson = @"
            {
                ""count"": 1302,
                ""results"": [
                    { ""name"": ""bulbasaur"", ""url"": ""https://pokeapi.co/api/v2/pokemon/1/"" },
                    { ""name"": ""ivysaur"",   ""url"": ""https://pokeapi.co/api/v2/pokemon/2/"" },
                    { ""name"": ""venusaur"",  ""url"": ""https://pokeapi.co/api/v2/pokemon/3/"" }
                ]
            }";

                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(listJson, Encoding.UTF8, "application/json")
                });
            }
            
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        }
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
