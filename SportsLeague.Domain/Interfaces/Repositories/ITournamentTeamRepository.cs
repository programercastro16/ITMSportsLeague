using SportsLeague.Domain.Entities;
namespace SportsLeague.Domain.Interfaces.Repositories;
public interface ITournamentTeamRepository : IGenericRepository<TournamentTeam>

{
    Task<TournamentTeam?> GetByTournamentAndTeamAsync(int tournamentId, int teamId);
    Task<IEnumerable<TournamentTeam>> GetByTournamentAsync(int tournamentId);
}