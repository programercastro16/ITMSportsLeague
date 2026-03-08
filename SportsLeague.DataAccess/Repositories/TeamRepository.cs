using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories;

public class TeamRepository : GenericRepository<Team>, ITeamRepository
{
    public TeamRepository(LeagueDbContext context) : base(context)
    {
    }

    public async Task<Team?> GetByNameAsync(string name)
    {
        return await _dbSet
            .FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower());
    }

    public async Task<IEnumerable<Team>> GetByCityAsync(string city)
    {
        return await _dbSet
            .Where(t => t.City.ToLower() == city.ToLower())
            .ToListAsync();
    }
}
