using PokemonCenter.Api.Services.Battle.Interfaces;
using PokemonCenter.Api.Services.Battle.Models;

namespace PokemonCenter.Api.Services.Battle;

public sealed class BattleService : IBattleService
{
    public BattleResult Simulate(BattlePokemon first, BattlePokemon second)
    {
        Validate(first);
        Validate(second);

        var (attacker, defender) = DetermineFirstAttacker(first, second);

        var turns = SimulateTurns(attacker, defender);

        return BuildResult(attacker, defender, turns);
    }

    private static (BattlePokemon Attacker, BattlePokemon Defender) DetermineFirstAttacker(BattlePokemon first, BattlePokemon second)
    {
        var attacker = first;
        var defender = second;

        if (second.Speed > first.Speed)
        {
            attacker = second;
            defender = first;
        }

        return (attacker, defender);
    }


    private static List<BattleTurnLog> SimulateTurns(BattlePokemon attacker, BattlePokemon defender)
    {
        var turns = new List<BattleTurnLog>();
        var turn = 1;

        while (attacker.Hp > 0 && defender.Hp > 0)
        {
            ExecuteTurn(attacker, defender, turn, turns);

            if (defender.Hp <= 0)
                break;

            (attacker, defender) = (defender, attacker);
            turn++;
        }

        return turns;
    }

    private static void ExecuteTurn(
        BattlePokemon attacker,
        BattlePokemon defender,
        int turn,
        List<BattleTurnLog> turns)
    {
        var attackerHpBefore = attacker.Hp;
        var defenderHpBefore = defender.Hp;

        var damage = CalculateDamage(attacker.Attack, defender.Defense);

        defender.Hp = Math.Max(0, defender.Hp - damage);

        turns.Add(new BattleTurnLog
        {
            Turn = turn,
            Attacker = attacker.Name,
            Defender = defender.Name,
            Damage = damage,

            AttackerHpBefore = attackerHpBefore,
            AttackerHpAfter = attacker.Hp,

            DefenderHpBefore = defenderHpBefore,
            DefenderHpAfter = defender.Hp
        });
    }

    private static BattleResult BuildResult(BattlePokemon lastAttacker, BattlePokemon lastDefender, List<BattleTurnLog> turns)
    {
        var winner = GetWinner(lastAttacker, lastDefender);
        var loser = GetLoser(winner, lastAttacker, lastDefender);

        return new BattleResult
        {
            Winner = winner.Name,
            WinnerHp = winner.Hp,

            Loser = loser.Name,
            LoserHp = loser.Hp,

            TotalTurns = turns.Count,
            Turns = turns
        };
    }

    private static BattlePokemon GetWinner(BattlePokemon attacker, BattlePokemon defender)
    {
        return attacker.Hp > 0 ? attacker : defender;
    }

    private static BattlePokemon GetLoser(BattlePokemon winner, BattlePokemon attacker, BattlePokemon defender)
    {
        return winner == attacker ? defender : attacker;
    }


    private static void Validate(BattlePokemon pokemon)
    {
        if (pokemon is null)
            throw new ArgumentNullException(nameof(pokemon));

        if (pokemon.Hp <= 0)
            throw new ArgumentException("Pokemon must start the battle with HP greater than 0");
    }

    private static int CalculateDamage(int attack, int defense)
    {
        var baseDamage = attack - defense;

        if (baseDamage < 1)
        {
            return 1;
        }

        return baseDamage;
    }
}
