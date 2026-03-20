using SportsLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.Domain.Interfaces.Services
{
    public interface IRefereeService

    {
        Task<IEnumerable<Referee>> GetAllAsync();
        Task<Referee?> GetByIdAsync(int id);
        Task<Referee> CreateAsync(Referee referee);
        Task UpdateAsync(int id, Referee referee);
        Task DeleteAsync(int id);
    }
}
