using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PokemonCenter.Api.Services.Pokedex;
using PokemonCenter.Api.Services.Pokedex.Interfaces;

namespace PokemonCenter.IntegrationTests.Infrastructure;

public enum PokeApiSimulationMode
{
    Success,
    HttpError,
    InvalidJson,
    NotFound
}

public class PokemonCenterApiFactory : WebApplicationFactory<Program>
{
    private readonly PokeApiSimulationMode _mode;

    public PokemonCenterApiFactory()
    {
        _mode = PokeApiSimulationMode.Success;
    }

    private PokemonCenterApiFactory(PokeApiSimulationMode mode)
    {
        _mode = mode;
    }

    public static PokemonCenterApiFactory CreateWithMode(PokeApiSimulationMode mode)
        => new PokemonCenterApiFactory(mode);

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IPokedexService>();
            
            HttpMessageHandler handler = _mode switch
            {
                PokeApiSimulationMode.Success => new PokeApiMock(),
                PokeApiSimulationMode.HttpError => new ErrorPokeApiMock(),
                PokeApiSimulationMode.InvalidJson => new InvalidJsonPokeApiMock(),
                PokeApiSimulationMode.NotFound => new NotFoundPokeApiHandler(),
                _ => new PokeApiMock(),
            };

            services.AddTransient<IPokedexService>(_ =>
            {
                var httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri("https://pokeapi.co/api/v2/")
                };
                return new PokedexService(httpClient);
            });
        });
    }
}
