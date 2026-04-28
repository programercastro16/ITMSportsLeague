namespace SportsLeague.API.DTOs.Response;
public class MatchResultResponseDTO

{
    public int Id { get; set; }
    public int MatchId { get; set; }
    public int HomeGoals { get; set; }
    public int AwayGoals { get; set; }
    public string? Observations { get; set; }
    public DateTime CreatedAt { get; set; }
}