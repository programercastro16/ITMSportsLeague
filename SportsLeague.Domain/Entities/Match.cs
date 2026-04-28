using SportsLeague.Domain.Enums;

namespace SportsLeague.Domain.Entities
{
    public class Match : AuditBase

    {
        public int TournamentId { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public int RefereeId { get; set; }
        public DateTime MatchDate { get; set; }
        public string Venue { get; set; } = string.Empty;
        public int Matchday { get; set; }
        public MatchStatus Status { get; set; } = MatchStatus.Scheduled;
        // Agregar dentro de Match: 
        // Relación 1:1 con resultado 
        public MatchResult? MatchResult { get; set; }
        // Relación 1:N con goles y tarjetas 
        public ICollection<Goal> Goals { get; set; } = new List<Goal>();
        public ICollection<Card> Cards { get; set; } = new List<Card>();

        // Navigation Properties

        public Tournament Tournament { get; set; } = null!;
        public Team HomeTeam { get; set; } = null!;
        public Team AwayTeam { get; set; } = null!;
        public Referee Referee { get; set; } = null!;

    }
}
