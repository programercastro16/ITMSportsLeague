using SportsLeague.Domain.Enums;

namespace SportsLeague.API.DTOs.Response
{
    public class PlayerResponseDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public int Number { get; set; }
        public PlayerPosition Position { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
