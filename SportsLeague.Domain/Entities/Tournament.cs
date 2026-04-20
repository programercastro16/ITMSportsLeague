using SportsLeague.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.Domain.Entities
{
    public class Tournament : AuditBase
    {
        public string Name { get; set; } = string.Empty;
        public string Season { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TournamentStatus Status { get; set; } = TournamentStatus.Pending;

        // Navigation Properties
        public ICollection<TournamentTeam> TournamentTeams { get; set; } = new List<TournamentTeam>();
        public ICollection<Match> Matches { get; set; } = new List<Match>();
        public ICollection<TournamentSponsor> TournamentSponsors { get; set; } = new List<TournamentSponsor>();

        // Agregar dentro de la clase Team, después de TournamentTeams:


        // NOTE: Home/Away matches belong to Team; Tournament only needs a single Matches collection
    }


}
