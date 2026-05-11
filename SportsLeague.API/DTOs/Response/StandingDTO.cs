namespace SportsLeague.API.DTOs.Response;
public class StandingDTO

{
    public int Position { get; set; }
    public int TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public int MatchesPlayed { get; set; }     // PJ 
    public int Wins { get; set; }              // PG 
    public int Draws { get; set; }             // PE 
    public int Losses { get; set; }            // PP 
    public int GoalsFor { get; set; }          // GF 
    public int GoalsAgainst { get; set; }      // GC 
    public int GoalDifference { get; set; }    // DG 
    public int Points { get; set; }            // Pts 

}