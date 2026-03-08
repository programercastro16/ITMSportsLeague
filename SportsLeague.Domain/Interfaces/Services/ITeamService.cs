using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Services
{
    public interface ITeamService
    {
        Task<IEnumerable<Team>> GetAllAsync();
        Task<Team?> GetByIdAsync(int id);
        Task<Team> CreateAsync(Team team);
        Task UpdateAsync(int id, Team team);
        Task DeleteAsync(int id);
    }

}
