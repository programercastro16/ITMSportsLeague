using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories;
public interface IMatchLineupRepository : IGenericRepository<MatchLineup>
{
    Task<IEnumerable<MatchLineup>> GetByMatchAsync(int matchId);
    Task<IEnumerable<MatchLineup>> GetByMatchAndTeamAsync(
    int matchId,
    int teamId);
    Task<bool> ExistsByMatchAndPlayerAsync(
    int matchId,
    int playerId);
    Task<int> CountStartersByMatchAndTeamAsync(
    int matchId,
    int teamId);
}