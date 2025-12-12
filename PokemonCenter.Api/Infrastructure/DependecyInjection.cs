using PokemonCenter.Api.Services;
using PokemonCenter.Api.Services.Interfaces;

namespace PokemonCenter.Api.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IPokemonService, PokemonService>(client =>
        {
            client.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
        });

        return services;
    }
}
