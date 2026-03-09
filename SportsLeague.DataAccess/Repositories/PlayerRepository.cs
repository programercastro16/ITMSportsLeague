using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories
{
    public class PlayerRepository : GenericRepository<Player>, IPlayerRepository

    {
        public PlayerRepository(LeagueDbContext context) : base(context)

        {

        }

        public async Task<IEnumerable<Player>> GetByTeamAsync(int teamId)

        {
            return await _dbSet
            .Where(p => p.TeamId == teamId)
            .Include(p => p.Team)
            .ToListAsync();
        }
        public async Task<Player?> GetByTeamAndNumberAsync(int teamId, int number)
        {
            return await _dbSet
            .FirstOrDefaultAsync(p => p.TeamId == teamId && p.Number == number);

        }
        public async Task<IEnumerable<Player>> GetAllWithTeamAsync()

        {
            return await _dbSet
            .Include(p => p.Team)
            .ToListAsync();
        }
        public async Task<Player?> GetByIdWithTeamAsync(int id)

        {
            return await _dbSet
            .Include(p => p.Team)
            .FirstOrDefaultAsync(p => p.Id == id);

        }

    }
}
