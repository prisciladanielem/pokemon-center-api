using PokemonCenter.Api.Dtos.Battle;

namespace PokemonCenter.Api.Dtos.Battle;

public class BattleTurnLogDto
{
    public int Turn { get; set; }
    public BattlePokemonDto Attacker { get; set; } = new();
    public BattlePokemonDto Defender { get; set; } = new();
    public int Damage { get; set; }
    public int DefenderHpBefore { get; set; }
    public int DefenderHpAfter { get; set; }
}
