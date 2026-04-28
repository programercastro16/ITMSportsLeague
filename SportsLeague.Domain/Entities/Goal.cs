using SportsLeague.Domain.Enums;
namespace SportsLeague.Domain.Entities;
public class Goal : AuditBase

{
    public int MatchId { get; set; }
    public int PlayerId { get; set; }
    public int Minute { get; set; }
    public GoalType Type { get; set; }

    // Navigation Properties 

    public Match Match { get; set; } = null!;
    public Player Player { get; set; } = null!;
}