using SportsLeague.Domain.Enums;

namespace SportsLeague.API.DTOs.Response
{
    public class MatchResponseDTO

    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public string TournamentName { get; set; } = string.Empty;
        public int HomeTeamId { get; set; }
        public string HomeTeamName { get; set; } = string.Empty;
        public int AwayTeamId { get; set; }
        public string AwayTeamName { get; set; } = string.Empty;
        public int RefereeId { get; set; }
        public string RefereeFullName { get; set; } = string.Empty;
        public DateTime MatchDate { get; set; }
        public string Venue { get; set; } = string.Empty;
        public int Matchday { get; set; }
        public MatchStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
