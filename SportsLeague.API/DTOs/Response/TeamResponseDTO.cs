namespace SportsLeague.API.DTOs.Response
{
    public class TeamResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Stadium { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
        public DateTime FoundedDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
