using Microsoft.EntityFrameworkCore;
using SportsLeague.Domain.Entities;

namespace SportsLeague.DataAccess.Context
{

    public class LeagueDbContext : DbContext
    {
        public LeagueDbContext(DbContextOptions<LeagueDbContext> options)
            : base(options)
        {
        }

        public DbSet<Team> Teams => Set<Team>();
        public DbSet<Player> Players => Set<Player>();
        public DbSet<Referee> Referees => Set<Referee>(); 
        public DbSet<Tournament> Tournaments => Set<Tournament>(); 
        public DbSet<TournamentTeam> TournamentTeams => Set<TournamentTeam>();
        public DbSet<Sponsor> Sponsors => Set<Sponsor>();
        public DbSet<TournamentSponsor> TournamentSponsors => Set<TournamentSponsor>();
        public DbSet<Match> Matches => Set<Match>();
        public DbSet<MatchResult> MatchResults => Set<MatchResult>();
        public DbSet<Goal> Goals => Set<Goal>();
        public DbSet<Card> Cards => Set<Card>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Name)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(t => t.City)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(t => t.Stadium)
                      .HasMaxLength(150);
                entity.Property(t => t.LogoUrl)
                      .HasMaxLength(500);
                entity.Property(t => t.CreatedAt)
                      .IsRequired();
                entity.Property(t => t.UpdatedAt)
                      .IsRequired(false);
                entity.HasIndex(t => t.Name)
                      .IsUnique();
            });

            // ── Team Configuration ── (configured above)
            // ── Player Configuration ──

            modelBuilder.Entity<Player>(entity =>

            {

                entity.HasKey(p => p.Id);
                entity.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(80);
                entity.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(80);
                entity.Property(p => p.BirthDate)
                .IsRequired();
                entity.Property(p => p.Number)
                .IsRequired();
                entity.Property(p => p.Position)
                .IsRequired();
                entity.Property(p => p.CreatedAt)
                .IsRequired();
                entity.Property(p => p.UpdatedAt)
                .IsRequired(false);

                // Relación 1:N con Team
                entity.HasOne(p => p.Team)
                .WithMany(t => t.Players)
                .HasForeignKey(p => p.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

                // Índice único compuesto: número de camiseta único por equipo
                entity.HasIndex(p => new { p.TeamId, p.Number }).IsUnique();

            });

            // ── Referee Configuration ──

            modelBuilder.Entity<Referee>(entity =>

            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.FirstName)
                .IsRequired()
                .HasMaxLength(80);
                entity.Property(r => r.LastName)
                .IsRequired()
                .HasMaxLength(80);
                entity.Property(r => r.Nationality)
                .IsRequired()
                .HasMaxLength(80);
                entity.Property(r => r.CreatedAt)
                .IsRequired();
                entity.Property(r => r.UpdatedAt)
                .IsRequired(false);
            });

            // ── Tournament Configuration ──

            modelBuilder.Entity<Tournament>(entity =>

            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(150);
                entity.Property(t => t.Season)
                .IsRequired()
                .HasMaxLength(20);
                entity.Property(t => t.StartDate)
                .IsRequired();
                entity.Property(t => t.EndDate)
                .IsRequired();
                entity.Property(t => t.Status)
                .IsRequired();
                entity.Property(t => t.CreatedAt)
                .IsRequired();
                entity.Property(t => t.UpdatedAt)
                .IsRequired(false);

            });

            // ── TournamentTeam Configuration ──

            modelBuilder.Entity<TournamentTeam>(entity =>

            {
                entity.HasKey(tt => tt.Id);
                entity.Property(tt => tt.RegisteredAt)
                .IsRequired();
                entity.Property(tt => tt.CreatedAt)
                .IsRequired();
                entity.Property(tt => tt.UpdatedAt)
                .IsRequired(false);

                // Relación con Tournament
                entity.HasOne(tt => tt.Tournament)
                .WithMany(t => t.TournamentTeams)
                .HasForeignKey(tt => tt.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);

                // Relación con Team
                entity.HasOne(tt => tt.Team)
                .WithMany(t => t.TournamentTeams)
                .HasForeignKey(tt => tt.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

                // Índice único compuesto: un equipo solo una vez por torneo
                entity.HasIndex(tt => new { tt.TournamentId, tt.TeamId })
                .IsUnique();
            });

            // ── Sponsor Configuration ──

            modelBuilder.Entity<Sponsor>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Name)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(s => s.ContactEmail)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(s => s.Phone)
                      .HasMaxLength(50);

                entity.Property(s => s.WebsiteUrl)
                      .HasMaxLength(250);

                entity.Property(s => s.Category)
                      .IsRequired();

                entity.Property(s => s.CreatedAt)
                      .IsRequired();

                entity.Property(s => s.UpdatedAt)
                      .IsRequired(false);

                entity.HasIndex(s => s.Name)
                      .IsUnique();
            });

            // ── TournamentSponsor Configuration (N:M) ──

            modelBuilder.Entity<TournamentSponsor>(entity =>
            {
                entity.HasKey(ts => ts.Id);

                entity.Property(ts => ts.ContractAmount)
                      .IsRequired();

                entity.Property(ts => ts.JoinedAt)
                      .IsRequired();

                entity.Property(ts => ts.CreatedAt)
                      .IsRequired();

                entity.Property(ts => ts.UpdatedAt)
                      .IsRequired(false);

                entity.HasOne(ts => ts.Tournament)
                      .WithMany(t => t.TournamentSponsors)
                      .HasForeignKey(ts => ts.TournamentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ts => ts.Sponsor)
                      .WithMany(s => s.TournamentSponsors)
                      .HasForeignKey(ts => ts.SponsorId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(ts => new { ts.TournamentId, ts.SponsorId })
                      .IsUnique();
            });

            // ── Match Configuration ──
            modelBuilder.Entity<Match>(entity =>

            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.MatchDate)
                .IsRequired();
                entity.Property(m => m.Venue)
                .HasMaxLength(150);
                entity.Property(m => m.Matchday)
                .IsRequired();
                entity.Property(m => m.Status)
                .IsRequired();
                entity.Property(m => m.CreatedAt)
                .IsRequired();
                entity.Property(m => m.UpdatedAt)

                .IsRequired(false);

                // Relación con Tournament (Cascade: eliminar torneo elimina partidos)
                entity.HasOne(m => m.Tournament)
                .WithMany(t => t.Matches)
                .HasForeignKey(m => m.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);


                // Relación con HomeTeam (Restrict: evita ciclo de cascada)
                entity.HasOne(m => m.HomeTeam)
                .WithMany(t => t.HomeMatches)
                .HasForeignKey(m => m.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

                // Relación con AwayTeam (Restrict: evita ciclo de cascada)
                entity.HasOne(m => m.AwayTeam)
                .WithMany(t => t.AwayMatches)
                .HasForeignKey(m => m.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

                // Relación con Referee (Restrict: no eliminar árbitro con partidos)
                entity.HasOne(m => m.Referee)
                .WithMany(r => r.Matches)
                .HasForeignKey(m => m.RefereeId)
                .OnDelete(DeleteBehavior.Restrict);

            });

            // ── MatchResult Configuration ── 

            modelBuilder.Entity<MatchResult>(entity =>

            {

                entity.HasKey(mr => mr.Id);
                entity.Property(mr => mr.HomeGoals).IsRequired();
                entity.Property(mr => mr.AwayGoals).IsRequired();
                entity.Property(mr => mr.Observations).HasMaxLength(500);
                entity.Property(mr => mr.CreatedAt).IsRequired();
                entity.Property(mr => mr.UpdatedAt).IsRequired(false);

                // Relación 1:1 con Match 
                entity.HasOne(mr => mr.Match)
                      .WithOne(m => m.MatchResult)
                      .HasForeignKey<MatchResult>(mr => mr.MatchId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Índice único en MatchId garantiza relación 1:1 
                entity.HasIndex(mr => mr.MatchId).IsUnique();

            });

            // ── Goal Configuration ── 

            modelBuilder.Entity<Goal>(entity =>

            {

                entity.HasKey(g => g.Id);
                entity.Property(g => g.Minute).IsRequired();
                entity.Property(g => g.Type).IsRequired();
                entity.Property(g => g.CreatedAt).IsRequired();
                entity.Property(g => g.UpdatedAt).IsRequired(false);
                entity.HasOne(g => g.Match)
                      .WithMany(m => m.Goals)
                      .HasForeignKey(g => g.MatchId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(g => g.Player)
                      .WithMany(p => p.Goals)
                      .HasForeignKey(g => g.PlayerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Card Configuration ── 

            modelBuilder.Entity<Card>(entity =>

            {

                entity.HasKey(c => c.Id);
                entity.Property(c => c.Minute).IsRequired();
                entity.Property(c => c.Type).IsRequired();
                entity.Property(c => c.CreatedAt).IsRequired();
                entity.Property(c => c.UpdatedAt).IsRequired(false);

                entity.HasOne(c => c.Match)
                      .WithMany(m => m.Cards)
                      .HasForeignKey(c => c.MatchId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.Player)
                      .WithMany(p => p.Cards)
                      .HasForeignKey(c => c.PlayerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });


        }

    }
}
    


