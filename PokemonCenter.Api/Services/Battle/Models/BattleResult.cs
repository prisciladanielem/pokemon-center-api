namespace PokemonCenter.Api.Services.Battle.Models;

public sealed class BattleResult
{
    public string Winner { get; init; } = string.Empty;
    public int WinnerHp { get; init; }
    public string Loser { get; init; } = string.Empty;
    public int LoserHp { get; init; }
    public int TotalTurns { get; init; }
    public IReadOnlyList<BattleTurnLog> Turns { get; init; } = Array.Empty<BattleTurnLog>();
}
