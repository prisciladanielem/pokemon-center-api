using PokemonCenter.Api.Dtos.Battle;

namespace PokemonCenter.Api.Services.Battle.Interfaces;

public interface IBattleSimulationService
{
    Task<BattleResultDto> SimulateAsync(BattleRequestDto request);
}
