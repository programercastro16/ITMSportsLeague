namespace SportsLeague.API.DTOs.Response;
public class TopScorerDTO

{
    public int PlayerId { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public string TeamName { get; set; } = string.Empty;
    public int Goals { get; set; }
    public int Penalties { get; set; }
    public int MatchesWithGoals { get; set; }
}