using PokemonCenter.Api.Dtos.Battle;

namespace PokemonCenter.Api.Dtos.Battle;

public class BattleResultDto
{
    public BattlePokemonDto Winner { get; set; } = new();
    public BattlePokemonDto Loser { get; set; } = new();
    public int TotalTurns { get; set; }
    public List<BattleTurnLogDto> Turns { get; set; } = new();
}
