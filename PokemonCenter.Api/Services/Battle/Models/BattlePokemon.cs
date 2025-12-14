namespace PokemonCenter.Api.Services.Battle.Models;

public class BattlePokemon
{
    public string Name { get; init; } = string.Empty;
    public int Hp { get; set; }  
    public int Attack { get; init; }
    public int Defense { get; init; }
    public int Speed { get; init; }
}
