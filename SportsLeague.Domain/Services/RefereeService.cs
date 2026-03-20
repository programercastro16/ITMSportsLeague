using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services;

public class RefereeService : IRefereeService

{
    private readonly IRefereeRepository _refereeRepository;
    private readonly ILogger<RefereeService> _logger;

    public RefereeService(
    IRefereeRepository refereeRepository,
    ILogger<RefereeService> logger)
    {
        _refereeRepository = refereeRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Referee>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all referees");
        return await _refereeRepository.GetAllAsync();
    }

    public async Task<Referee?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving referee with ID: {RefereeId}", id);
        var referee = await _refereeRepository.GetByIdAsync(id);
        if (referee == null)
            _logger.LogWarning("Referee with ID {RefereeId} not found", id);
        return referee;
    }

    public async Task<Referee> CreateAsync(Referee referee)
    {
        _logger.LogInformation(
        "Creating referee: {FirstName} {LastName}",
        referee.FirstName, referee.LastName);
        return await _refereeRepository.CreateAsync(referee);
    }

    public async Task UpdateAsync(int id, Referee referee)
    {
        var existing = await _refereeRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"No se encontró el árbitro con ID {id}");

        existing.FirstName = referee.FirstName;
        existing.LastName = referee.LastName;
        existing.Nationality = referee.Nationality;

        _logger.LogInformation("Updating referee with ID: {RefereeId}", id);
        await _refereeRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        var exists = await _refereeRepository.ExistsAsync(id);
        if (!exists)
            throw new KeyNotFoundException($"No se encontró el árbitro con ID {id}");
        _logger.LogInformation("Deleting referee with ID: {RefereeId}", id);
        await _refereeRepository.DeleteAsync(id);
    }

}