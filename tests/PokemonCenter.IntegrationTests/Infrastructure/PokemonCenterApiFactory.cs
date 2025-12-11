using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using PokemonCenter.Api.Services;
using PokemonCenter.Api.Services.Interfaces;

namespace PokemonCenter.IntegrationTests.Infrastructure;

public enum PokeApiSimulationMode
{
    Success,
    HttpError,
    InvalidJson
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
            HttpMessageHandler handler = _mode switch
            {
                PokeApiSimulationMode.Success     => new PokeApiMock(),
                PokeApiSimulationMode.HttpError   => new ErrorPokeApiMock(),
                PokeApiSimulationMode.InvalidJson => new InvalidJsonPokeApiMock(),
                _ => new PokeApiMock(),
            };

            services.AddTransient<IPokemonService>(_ =>
            {
                var httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri("https://pokeapi.co/api/v2/")
                };
                return new PokemonService(httpClient);
            });
        });
    }
}
