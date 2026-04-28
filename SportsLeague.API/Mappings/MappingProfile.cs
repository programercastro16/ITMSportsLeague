using AutoMapper;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
 
 namespace SportsLeague.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Team mappings
        CreateMap<TeamRequestDTO, Team>();
        CreateMap<Team, TeamResponseDTO>();

        // Player mappings
        CreateMap<PlayerRequestDTO, Player>();
        CreateMap<Player, PlayerResponseDTO>()
            .ForMember(
                dest => dest.TeamName,
                opt => opt.MapFrom(src => src.Team.Name)); 

        // Referee mappings
        CreateMap<RefereeRequestDTO, Referee>();
        CreateMap<Referee, RefereeResponseDTO>();

        // Tournament mappings
        CreateMap<TournamentRequestDTO, Tournament>();
        CreateMap<Tournament, TournamentResponseDTO>()
            .ForMember(
                dest => dest.TeamsCount,
                opt => opt.MapFrom(src =>
                    src.TournamentTeams != null ? src.TournamentTeams.Count : 0));

        // Sponsor mappings
        CreateMap<SponsorRequestDTO, Sponsor>();
        CreateMap<Sponsor, SponsorResponseDTO>();

        CreateMap<TournamentSponsorRequestDTO, TournamentSponsor>();
        CreateMap<TournamentSponsor, TournamentSponsorResponseDTO>()
            .ForMember(dest => dest.TournamentName, opt => opt.MapFrom(src => src.Tournament.Name))
            .ForMember(dest => dest.SponsorName, opt => opt.MapFrom(src => src.Sponsor.Name));

        // Match mappings

        CreateMap<MatchRequestDTO, Match>();
        CreateMap<Match, MatchResponseDTO>()
        .ForMember(dest => dest.TournamentName,
        opt => opt.MapFrom(src => src.Tournament.Name))
        .ForMember(dest => dest.HomeTeamName,
        opt => opt.MapFrom(src => src.HomeTeam.Name))
        .ForMember(dest => dest.AwayTeamName,
        opt => opt.MapFrom(src => src.AwayTeam.Name))
        .ForMember(dest => dest.RefereeFullName,
        opt => opt.MapFrom(src =>
        src.Referee.FirstName + " " + src.Referee.LastName));

        // MatchResult mappings 
        CreateMap<MatchResultRequestDTO, MatchResult>();
        CreateMap<MatchResult, MatchResultResponseDTO>();

        // Goal mappings 
        CreateMap<GoalRequestDTO, Goal>();
        CreateMap<Goal, GoalResponseDTO>()
            .ForMember(dest => dest.PlayerName,
                opt => opt.MapFrom(src =>
                    src.Player.FirstName + " " + src.Player.LastName));

        // Card mappings 
        CreateMap<CardRequestDTO, Card>();
        CreateMap<Card, CardResponseDTO>()
            .ForMember(dest => dest.PlayerName,
                opt => opt.MapFrom(src =>
                    src.Player.FirstName + " " + src.Player.LastName));
    }
}
