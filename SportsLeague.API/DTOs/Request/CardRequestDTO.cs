using SportsLeague.Domain.Enums;

namespace SportsLeague.API.DTOs.Request;
public class CardRequestDTO

{
    public int PlayerId { get; set; }
    public int Minute { get; set; }
    public CardType Type { get; set; }
}