namespace PokemonCenter.Api.Services.Battle.Exceptions;

public sealed class PokemonNotFoundException : Exception
{
    public PokemonNotFoundException(string idOrName)
        : base($"Pokémon '{idOrName}' was not found.")
    {
    }
}
