namespace PokemonCenter.Api.Services.Battle.Models;

public sealed class BattleTurnLog
{
    public int Turn { get; init; }
    public string Attacker { get; init; } = string.Empty;
    public string Defender { get; init; } = string.Empty;
    public int Damage { get; init; }
    public int AttackerHpBefore { get; init; }
    public int AttackerHpAfter { get; init; }
    public int DefenderHpBefore { get; init; }
    public int DefenderHpAfter { get; init; }
}
