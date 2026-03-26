using SportsLeague.Domain.Enums;

namespace SportsLeague.API.DTOs.Request
{
    public class UpdateStatusDTO
    {
        public TournamentStatus Status { get; set; }
    }

}
