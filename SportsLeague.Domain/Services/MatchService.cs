using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services
{
    public class MatchService : IMatchService

    {

        private readonly IMatchRepository _matchRepository;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly ITournamentTeamRepository _tournamentTeamRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IRefereeRepository _refereeRepository;
        private readonly ILogger<MatchService> _logger;

        public MatchService(

        IMatchRepository matchRepository,
        ITournamentRepository tournamentRepository,
        ITournamentTeamRepository tournamentTeamRepository,
        ITeamRepository teamRepository,
        IRefereeRepository refereeRepository,
        ILogger<MatchService> logger)

        {
            _matchRepository = matchRepository;
            _tournamentRepository = tournamentRepository;
            _tournamentTeamRepository = tournamentTeamRepository;
            _teamRepository = teamRepository;
            _refereeRepository = refereeRepository;
            _logger = logger;
        }


        public async Task<IEnumerable<Match>> GetAllByTournamentAsync(int tournamentId)

        {
            var tournament = await _tournamentRepository.GetByIdAsync(tournamentId);
            if (tournament == null)
                throw new KeyNotFoundException(
                $"No se encontró el torneo con ID {tournamentId}");
            return await _matchRepository.GetByTournamentWithDetailsAsync(tournamentId);

        }


        public async Task<Match?> GetByIdAsync(int id)

        {
            _logger.LogInformation("Retrieving match with ID: {MatchId}", id);
            return await _matchRepository.GetByIdWithDetailsAsync(id);
        }


        public async Task<Match> CreateAsync(Match match)

        {
            // 1. Validar que el torneo existe y está en InProgress
            var tournament = await _tournamentRepository.GetByIdAsync(match.TournamentId);
            if (tournament == null)
                throw new KeyNotFoundException(
                $"No se encontró el torneo con ID {match.TournamentId}");

            if (tournament.Status != TournamentStatus.InProgress)
                throw new InvalidOperationException(
                "Solo se pueden programar partidos en torneos con estado InProgress");

            // 2. Validar que los equipos son diferentes
            if (match.HomeTeamId == match.AwayTeamId)
                throw new InvalidOperationException(
                "El equipo local y visitante deben ser diferentes");

            // 3. Validar que ambos equipos existen
            var homeTeamExists = await _teamRepository.ExistsAsync(match.HomeTeamId);
            if (!homeTeamExists)
                throw new KeyNotFoundException(
                $"No se encontró el equipo local con ID {match.HomeTeamId}");

            var awayTeamExists = await _teamRepository.ExistsAsync(match.AwayTeamId);
            if (!awayTeamExists)
                throw new KeyNotFoundException(
                $"No se encontró el equipo visitante con ID {match.AwayTeamId}");

            // 4. Validar que ambos equipos están inscritos en el torneo
            var homeEnrolled = await _tournamentTeamRepository
            .GetByTournamentAndTeamAsync(match.TournamentId, match.HomeTeamId);
            if (homeEnrolled == null)
                throw new InvalidOperationException(
                "El equipo local no está inscrito en este torneo");
            var awayEnrolled = await _tournamentTeamRepository
            .GetByTournamentAndTeamAsync(match.TournamentId, match.AwayTeamId);
            if (awayEnrolled == null)
                throw new InvalidOperationException(
                "El equipo visitante no está inscrito en este torneo");

            // 5. Validar que el árbitro existe
            var refereeExists = await _refereeRepository.ExistsAsync(match.RefereeId);
            if (!refereeExists)
                throw new KeyNotFoundException(
                $"No se encontró el árbitro con ID {match.RefereeId}");

            match.Status = MatchStatus.Scheduled;
            _logger.LogInformation(

            "Creating match: Team {Home} vs Team {Away} in Tournament {Tournament}",
            match.HomeTeamId, match.AwayTeamId, match.TournamentId);
            return await _matchRepository.CreateAsync(match);
        }

        public async Task UpdateAsync(int id, Match match)

        {
            var existing = await _matchRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"No se encontró el partido con ID {id}");
            if (existing.Status != MatchStatus.Scheduled)
                throw new InvalidOperationException(
                "Solo se pueden editar partidos con estado Scheduled");
            // Mismas validaciones que en Create
            if (match.HomeTeamId == match.AwayTeamId)
                throw new InvalidOperationException(
                "El equipo local y visitante deben ser diferentes");
            var homeEnrolled = await _tournamentTeamRepository
            .GetByTournamentAndTeamAsync(existing.TournamentId, match.HomeTeamId);
            if (homeEnrolled == null)
                throw new InvalidOperationException(
                "El equipo local no está inscrito en este torneo");

            var awayEnrolled = await _tournamentTeamRepository
            .GetByTournamentAndTeamAsync(existing.TournamentId, match.AwayTeamId);
            if (awayEnrolled == null)
                throw new InvalidOperationException(
                "El equipo visitante no está inscrito en este torneo");

            var refereeExists = await _refereeRepository.ExistsAsync(match.RefereeId);
            if (!refereeExists)
                throw new KeyNotFoundException(
                $"No se encontró el árbitro con ID {match.RefereeId}");

            existing.HomeTeamId = match.HomeTeamId;
            existing.AwayTeamId = match.AwayTeamId;
            existing.RefereeId = match.RefereeId;
            existing.MatchDate = match.MatchDate;
            existing.Venue = match.Venue;
            existing.Matchday = match.Matchday;

            _logger.LogInformation("Updating match with ID: {MatchId}", id);
            await _matchRepository.UpdateAsync(existing);
        }


        public async Task DeleteAsync(int id)

        {
            var existing = await _matchRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"No se encontró el partido con ID {id}");
            if (existing.Status != MatchStatus.Scheduled)
                throw new InvalidOperationException(
                "Solo se pueden eliminar partidos con estado Scheduled");
            _logger.LogInformation("Deleting match with ID: {MatchId}", id);
            await _matchRepository.DeleteAsync(id);
        }

        public async Task UpdateStatusAsync(int id, MatchStatus newStatus)

        {
            var match = await _matchRepository.GetByIdAsync(id);
            if (match == null)
                throw new KeyNotFoundException($"No se encontró el partido con ID {id}");
            var validTransition = (match.Status, newStatus) switch
            {
                (MatchStatus.Scheduled, MatchStatus.InProgress) => true,
                (MatchStatus.InProgress, MatchStatus.Finished) => true,
                (MatchStatus.Scheduled, MatchStatus.Suspended) => true,
                (MatchStatus.InProgress, MatchStatus.Suspended) => true,
                _ => false
            };

            if (!validTransition)
                throw new InvalidOperationException(
                $"No se puede cambiar de {match.Status} a {newStatus}");
            match.Status = newStatus;
            _logger.LogInformation(
            "Updating match {MatchId} status to {NewStatus}", id, newStatus);
            await _matchRepository.UpdateAsync(match);

        }

    }
}
