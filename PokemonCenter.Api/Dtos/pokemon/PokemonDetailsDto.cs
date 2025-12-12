using PokemonCenter.Api.Dtos.pokemon;

namespace PokemonCenter.Api.Dtos.Pokemon;

public class PokemonDetailsDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Height { get; set; }

    public int Weight { get; set; }

    public List<string> Types { get; set; } = new();

    public List<string> Abilities { get; set; } = new();

    public List<PokemonStatDto> Stats { get; set; } = new();

    public PokemonSpritesDto Sprites { get; set; } = new();
}
