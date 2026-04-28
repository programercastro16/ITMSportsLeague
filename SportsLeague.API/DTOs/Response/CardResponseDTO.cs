using SportsLeague.Domain.Enums;

namespace SportsLeague.API.DTOs.Response;
public class CardResponseDTO

{
    public int Id { get; set; }
    public int MatchId { get; set; }
    public int PlayerId { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public int Minute { get; set; }
    public CardType Type { get; set; }
    public DateTime CreatedAt { get; set; }
}