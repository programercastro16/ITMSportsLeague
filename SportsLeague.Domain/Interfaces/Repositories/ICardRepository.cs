using SportsLeague.Domain.Entities;
namespace SportsLeague.Domain.Interfaces.Repositories;

public interface ICardRepository : IGenericRepository<Card>

{
    Task<IEnumerable<Card>> GetByMatchAsync(int matchId);
    Task<IEnumerable<Card>> GetByMatchWithDetailsAsync(int matchId);
}