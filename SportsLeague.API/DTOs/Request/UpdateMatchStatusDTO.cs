using SportsLeague.Domain.Enums;

namespace SportsLeague.API.DTOs.Request;

public class UpdateMatchStatusDTO

{
   public MatchStatus Status { get; set; }
}