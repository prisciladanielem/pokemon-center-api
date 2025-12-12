using System.Text.Json.Serialization;

namespace PokemonCenter.Api.Infrastructure.External.PokeApi;

public class PokeApiPokemonDetails
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("weight")]
    public int Weight { get; set; }

    [JsonPropertyName("types")]
    public List<PokeApiPokemonTypeSlot> Types { get; set; } = new();

    [JsonPropertyName("abilities")]
    public List<PokeApiPokemonAbilitySlot> Abilities { get; set; } = new();

    [JsonPropertyName("stats")]
    public List<PokeApiPokemonStat> Stats { get; set; } = new();

    [JsonPropertyName("sprites")]
    public PokeApiPokemonSprites Sprites { get; set; } = new();
}


public class PokeApiNamedResource
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
}


public class PokeApiPokemonTypeSlot
{
    [JsonPropertyName("slot")]
    public int Slot { get; set; }

    [JsonPropertyName("type")]
    public PokeApiNamedResource Type { get; set; } = new();
}


public class PokeApiPokemonAbilitySlot
{
    [JsonPropertyName("is_hidden")]
    public bool IsHidden { get; set; }

    [JsonPropertyName("slot")]
    public int Slot { get; set; }

    [JsonPropertyName("ability")]
    public PokeApiNamedResource Ability { get; set; } = new();
}


public class PokeApiPokemonStat
{
    [JsonPropertyName("base_stat")]
    public int BaseStat { get; set; }

    [JsonPropertyName("effort")]
    public int Effort { get; set; }

    [JsonPropertyName("stat")]
    public PokeApiNamedResource Stat { get; set; } = new();
}

public class PokeApiPokemonSprites
{
    [JsonPropertyName("front_default")]
    public string? FrontDefault { get; set; }

    [JsonPropertyName("back_default")]
    public string? BackDefault { get; set; }

    [JsonPropertyName("front_shiny")]
    public string? FrontShiny { get; set; }

    [JsonPropertyName("back_shiny")]
    public string? BackShiny { get; set; }

    [JsonPropertyName("front_female")]
    public string? FrontFemale { get; set; }

    [JsonPropertyName("back_female")]
    public string? BackFemale { get; set; }

    [JsonPropertyName("front_shiny_female")]
    public string? FrontShinyFemale { get; set; }

    [JsonPropertyName("back_shiny_female")]
    public string? BackShinyFemale { get; set; }

    [JsonPropertyName("other")]
    public PokeApiPokemonOtherSprites? Other { get; set; }
}

public class PokeApiPokemonOtherSprites
{
    [JsonPropertyName("official-artwork")]
    public PokeApiPokemonOfficialArtwork? OfficialArtwork { get; set; }
}


public class PokeApiPokemonOfficialArtwork
{
    [JsonPropertyName("front_default")]
    public string? FrontDefault { get; set; }
}
