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
            const string json = @"
            {
                ""count"": 1302,
                ""results"": [
                    { ""name"": ""bulbasaur"", ""url"": ""https://pokeapi.co/api/v2/pokemon/1/"" },
                    { ""name"": ""ivysaur"",   ""url"": ""https://pokeapi.co/api/v2/pokemon/2/"" },
                    { ""name"": ""venusaur"",  ""url"": ""https://pokeapi.co/api/v2/pokemon/3/"" }
                ]
            }";

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            return Task.FromResult(response);
        }
    }

    public sealed class ErrorPokeApiMock : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            return Task.FromResult(response);
        }
    }

    public sealed class InvalidJsonPokeApiMock : HttpMessageHandler
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
}