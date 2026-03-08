using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface ITeamRepository : IGenericRepository<Team>
    {
        Task<Team?> GetByNameAsync(string name);
        Task<IEnumerable<Team>> GetByCityAsync(string city);
    }

}
