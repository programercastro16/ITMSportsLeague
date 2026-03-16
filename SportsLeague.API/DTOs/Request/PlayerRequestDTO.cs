using SportsLeague.Domain.Enums;

namespace SportsLeague.API.DTOs.Request
{
    public class PlayerRequestDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public int Number { get; set; }
        public PlayerPosition Position { get; set; }
        public int TeamId { get; set; }
    }

}
