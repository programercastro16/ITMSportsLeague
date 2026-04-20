namespace SportsLeague.API.DTOs.Request
{
    public class MatchRequestDTO

    {

        public int TournamentId { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public int RefereeId { get; set; }
        public DateTime MatchDate { get; set; }
        public string Venue { get; set; } = string.Empty;
        public int Matchday { get; set; }

    }
}
