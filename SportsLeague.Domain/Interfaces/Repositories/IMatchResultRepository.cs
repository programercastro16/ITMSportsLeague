using SportsLeague.Domain.Entities;
namespace SportsLeague.Domain.Interfaces.Repositories;
public interface IMatchResultRepository : IGenericRepository<MatchResult>

{
    Task<MatchResult?> GetByMatchIdAsync(int matchId);
}