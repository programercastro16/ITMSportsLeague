using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;
namespace SportsLeague.Domain.Services;

public class MatchLineupService : IMatchLineupService
{
    private readonly IMatchLineupRepository _matchLineupRepository;
    private readonly IMatchRepository _matchRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly ILogger<MatchLineupService> _logger;
    public MatchLineupService(
    IMatchLineupRepository matchLineupRepository,
    IMatchRepository matchRepository,
    IPlayerRepository playerRepository,
    ILogger<MatchLineupService> logger)
    {
        _matchLineupRepository = matchLineupRepository;
        _matchRepository = matchRepository;
        _playerRepository = playerRepository;
        _logger = logger;
    }
    public async Task<MatchLineup> CreateAsync(MatchLineup lineup)
    {
        // V1
        var match = await _matchRepository.GetByIdAsync(lineup.MatchId);
        if (match == null)
        {
            throw new KeyNotFoundException(
            $"No se encontró el partido con ID {lineup.MatchId}");
            
        }
        // V6
        if (match.Status != MatchStatus.Scheduled)
        {
            throw new InvalidOperationException(
            "Solo se pueden registrar alineaciones en partidos Scheduled");
        }
        // V2
        var player = await _playerRepository.GetByIdAsync(lineup.PlayerId);
        if (player == null)
        {
            throw new KeyNotFoundException(
            $"No se encontró el jugador con ID {lineup.PlayerId}");
        }
        // V3
        var belongsToMatch =
        player.TeamId == match.HomeTeamId ||
        player.TeamId == match.AwayTeamId;
        if (!belongsToMatch)
        {
            throw new InvalidOperationException(
            "El jugador no pertenece a ninguno de los equipos del partido");
        }
        // V4
        var alreadyExists = await _matchLineupRepository
        .ExistsByMatchAndPlayerAsync(
        lineup.MatchId,
        lineup.PlayerId);
        if (alreadyExists)
        {
            throw new InvalidOperationException(
            "El jugador ya está registrado en la alineación de este partido");
        }
        // V5
        if (lineup.IsStarter)
        {
            var startersCount = await _matchLineupRepository
            .CountStartersByMatchAndTeamAsync(
            lineup.MatchId,
            
            player.TeamId);
            if (startersCount >= 11)
            {
                throw new InvalidOperationException(
                "El equipo ya tiene 11 titulares registrados en este partido");
            }
        }
        _logger.LogInformation(
        "Registering player {PlayerId} in lineup for match {MatchId}",
        lineup.PlayerId,
        lineup.MatchId);
        return await _matchLineupRepository.CreateAsync(lineup);
    }
    public async Task<IEnumerable<MatchLineup>> GetByMatchAsync(int matchId)
    {
        return await _matchLineupRepository.GetByMatchAsync(matchId);
    }
    public async Task<IEnumerable<MatchLineup>> GetByMatchAndTeamAsync(
    int matchId,
    int teamId)
    {
        return await _matchLineupRepository
        .GetByMatchAndTeamAsync(matchId, teamId);
    }
    public async Task DeleteAsync(int matchId, int id)
    {
        var lineups = await _matchLineupRepository.GetByMatchAsync(matchId);
        var lineup = lineups.FirstOrDefault(l => l.Id == id);
        if (lineup == null)
        {
            throw new KeyNotFoundException(
            $"No se encontró la alineación con ID {id}");
        }
        await _matchLineupRepository.DeleteAsync(id);
    }
}
