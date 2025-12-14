using PokemonCenter.Api.Dtos.Battle;
using PokemonCenter.Api.Dtos.Pokemon;
using PokemonCenter.Api.Services.Battle.Models;

namespace PokemonCenter.Api.Services.Battle.Mappers;

public static class BattleResultMapper
{
    public static BattleResultDto MapResultToDto(BattleResult result, IReadOnlyList<PokemonDetailsDto> participants)
    {
        return new BattleResultDto
        {
            Winner = new BattlePokemonDto
            {
                Id = ResolveIdByName(result.Winner, participants),
                Name = result.Winner
            },
            Loser = new BattlePokemonDto
            {
                Id = ResolveIdByName(result.Loser, participants),
                Name = result.Loser
            },
            TotalTurns = result.TotalTurns,
            Turns = result.Turns.Select(t => MapTurnLogToDto(t, participants)).ToList()
        };
    }

    private static int ResolveIdByName(string name, IReadOnlyList<PokemonDetailsDto> participants)
    {
        var pokemon = participants.First(p =>
            string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));

        return pokemon.Id;
    }

    private static BattleTurnLogDto MapTurnLogToDto(BattleTurnLog turn, IReadOnlyList<PokemonDetailsDto> participants)
    {
        return new BattleTurnLogDto
        {
            Turn = turn.Turn,

            Attacker = new BattlePokemonDto
            {
                Id = ResolveIdByName(turn.Attacker, participants),
                Name = turn.Attacker
            },
            Defender = new BattlePokemonDto
            {
                Id = ResolveIdByName(turn.Defender, participants),
                Name = turn.Defender
            },

            Damage = turn.Damage,

            DefenderHpBefore = turn.DefenderHpBefore,
            DefenderHpAfter = turn.DefenderHpAfter
        };
    }

}
