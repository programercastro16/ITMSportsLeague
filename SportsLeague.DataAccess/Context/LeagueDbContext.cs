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

            // ── Team Configuration ──

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

        }

    }
}
    


