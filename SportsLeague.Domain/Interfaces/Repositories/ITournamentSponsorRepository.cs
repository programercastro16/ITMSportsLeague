using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface ITournamentSponsorRepository : IGenericRepository<TournamentSponsor>
    {
        Task<TournamentSponsor?> GetBySponsorAndTournamentAsync(int sponsorId, int tournamentId);
        Task<IEnumerable<TournamentSponsor>> GetBySponsorIdAsync(int sponsorId);
        Task<IEnumerable<TournamentSponsor>> GetByTournamentIdAsync(int tournamentId);
    }
}
