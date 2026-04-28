using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories;

public class GoalRepository : GenericRepository<Goal>, IGoalRepository

{

    public GoalRepository(LeagueDbContext context) : base(context) { }

    public async Task<IEnumerable<Goal>> GetByMatchAsync(int matchId)

    {
        return await _dbSet
            .Where(g => g.MatchId == matchId)
            .OrderBy(g => g.Minute)
            .ToListAsync();

    }

    public async Task<IEnumerable<Goal>> GetByMatchWithDetailsAsync(int matchId)

    {

        return await _dbSet
            .Where(g => g.MatchId == matchId)
            .Include(g => g.Player)
            .OrderBy(g => g.Minute)
            .ToListAsync();
    }

}