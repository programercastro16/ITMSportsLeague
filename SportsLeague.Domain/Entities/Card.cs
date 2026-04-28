using SportsLeague.Domain.Enums;

namespace SportsLeague.Domain.Entities;
public class Card : AuditBase

{

    public int MatchId { get; set; }

    public int PlayerId { get; set; }

    public int Minute { get; set; }

    public CardType Type { get; set; }

    // Navigation Properties 

    public Match Match { get; set; } = null!;

    public Player Player { get; set; } = null!;

}
