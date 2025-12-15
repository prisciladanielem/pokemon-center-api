using PokemonCenter.Api.Dtos.Battle;
using PokemonCenter.Api.Dtos.Pokemon;
using PokemonCenter.Api.Services.Battle.Exceptions;
using PokemonCenter.Api.Services.Battle.Interfaces;
using PokemonCenter.Api.Services.Battle.Mappers;
using PokemonCenter.Api.Services.Pokedex.Interfaces;

namespace PokemonCenter.Api.Services.Battle;

public class BattleSimulationService : IBattleSimulationService
{
    private readonly IBattleService _battleService;
    private readonly IPokedexService _pokedexService;

    public BattleSimulationService(IBattleService battleService, IPokedexService pokedexService)
    {
        _battleService = battleService;
        _pokedexService = pokedexService;
    }

    public async Task<BattleResultDto> SimulateAsync(BattleRequestDto request)
    {
        var (firstDetails, secondDetails) = await GetParticipantsAsync(request);

        var firstBattlePokemon = BattlePokemonMapper.MapToBattlePokemon(firstDetails);
        var secondBattlePokemon = BattlePokemonMapper.MapToBattlePokemon(secondDetails);

        var result = _battleService.Simulate(firstBattlePokemon, secondBattlePokemon);

        return BattleResultMapper.MapResultToDto(result, new[] { firstDetails, secondDetails });
    }

    private async Task<(PokemonDetailsDto First, PokemonDetailsDto Second)> GetParticipantsAsync(BattleRequestDto request)
    {
        var tasks = new[]
        {
            _pokedexService.GetPokemonDetailsAsync(request.FirstPokemon),
            _pokedexService.GetPokemonDetailsAsync(request.SecondPokemon)
        };

        var details = await Task.WhenAll(tasks);

        var first = details[0] ?? throw new PokemonNotFoundException(request.FirstPokemon);

        var second = details[1] ?? throw new PokemonNotFoundException(request.SecondPokemon);

        return (first, second);
    }
}
