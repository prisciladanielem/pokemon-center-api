using PokemonCenter.Api.Dtos.Pokemon;
using PokemonCenter.Api.Services.Battle.Models;

namespace PokemonCenter.Api.Services.Battle.Mappers;

public static class BattlePokemonMapper
{
    public static BattlePokemon MapToBattlePokemon(PokemonDetailsDto details)
    {
        return new BattlePokemon
        {
            Name = details.Name,
            Hp = GetStat(details, "hp"),
            Attack = GetStat(details, "attack"),
            Defense = GetStat(details, "defense"),
            Speed = GetStat(details, "speed")
        };
    }

    private static int GetStat(PokemonDetailsDto details, string statName)
    {
        var stat = details.Stats
            .First(s => s.Name.Equals(statName, StringComparison.OrdinalIgnoreCase));

        return stat.BaseValue;
    }
}
