using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories
{
    public class MatchRepository : GenericRepository<Match>, IMatchRepository

    {
        public MatchRepository(LeagueDbContext context) : base(context)

        {

        }

        public async Task<IEnumerable<Match>> GetByTournamentAsync(int tournamentId)

        {
            return await _dbSet
            .Where(m => m.TournamentId == tournamentId)
            .OrderBy(m => m.Matchday)
            .ThenBy(m => m.MatchDate)
            .ToListAsync();
        }

        public async Task<IEnumerable<Match>> GetByTeamAsync(int teamId)

        {
            return await _dbSet
            .Where(m => m.HomeTeamId == teamId || m.AwayTeamId == teamId)
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .OrderBy(m => m.MatchDate)
            .ToListAsync();
        }
        public async Task<Match?> GetByIdWithDetailsAsync(int id)

        {
            return await _dbSet
            .Include(m => m.Tournament)
            .Include(m => m.HomeTeam)
            .ThenInclude(ht => ht.Players) // cargar jugadores del equipo local
            .Include(m => m.AwayTeam)
            .ThenInclude(at => at.Players) // cargar jugadores del equipo visitante
            .Include(m => m.Referee)
            .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Match>> GetByTournamentWithDetailsAsync(

        int tournamentId)

        {
            return await _dbSet
            .Where(m => m.TournamentId == tournamentId)
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Referee)
            .OrderBy(m => m.Matchday)
            .ThenBy(m => m.MatchDate)
            .ToListAsync();
        }

    }
}
