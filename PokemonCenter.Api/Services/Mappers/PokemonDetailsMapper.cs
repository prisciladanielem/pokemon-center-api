using PokemonCenter.Api.Dtos.pokemon;
using PokemonCenter.Api.Dtos.Pokemon;
using PokemonCenter.Api.Infrastructure.External.PokeApi;

namespace PokemonCenter.Api.Services.Mappers
{
    public static class PokemonDetailsMapper
    {
        public static PokemonDetailsDto MapToPokemonDetailsDto(PokeApiPokemonDetails source)
        {
            return new PokemonDetailsDto
            {
                Id = source.Id,
                Name = source.Name,
                Height = source.Height,
                Weight = source.Weight,

                Types = source.Types
                    .OrderBy(t => t.Slot)
                    .Select(t => t.Type.Name)
                    .ToList(),

                Abilities = source.Abilities
                    .OrderBy(a => a.Slot)
                    .Select(a => a.Ability.Name)
                    .ToList(),

                Stats = source.Stats
                    .Select(s => new PokemonStatDto
                    {
                        Name = s.Stat.Name,
                        BaseValue = s.BaseStat
                    })
                    .ToList(),

                Sprites = new PokemonSpritesDto
                {
                    FrontDefault = source.Sprites.FrontDefault,
                    BackDefault = source.Sprites.BackDefault,
                    FrontShiny = source.Sprites.FrontShiny,
                    BackShiny = source.Sprites.BackShiny,
                    OfficialArtwork = source.Sprites.Other?.OfficialArtwork?.FrontDefault
                }
            };
        }
    }
}
