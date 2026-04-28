namespace SportsLeague.API.DTOs.Request;
public class MatchResultRequestDTO

{
    public int HomeGoals { get; set; }
    public int AwayGoals { get; set; }
    public string? Observations { get; set; }
}