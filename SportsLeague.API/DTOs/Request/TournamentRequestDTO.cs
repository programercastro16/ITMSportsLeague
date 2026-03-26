namespace SportsLeague.API.DTOs.Request
{
    public class TournamentRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Season { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

}
