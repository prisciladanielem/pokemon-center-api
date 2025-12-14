using PokemonCenter.Api.Services.Battle.Models;

namespace PokemonCenter.Api.Services.Battle.Interfaces;

public interface IBattleService
{
    BattleResult Simulate(BattlePokemon first, BattlePokemon second);
}
