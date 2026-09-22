import { DatePipe } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { forkJoin } from 'rxjs';
import { TournamentStatus } from '../../core/models/enums';
import { PlayerService } from '../../core/services/player.service';
import { RefereeService } from '../../core/services/referee.service';
import { SponsorService } from '../../core/services/sponsor.service';
import { TeamService } from '../../core/services/team.service';
import { TournamentService } from '../../core/services/tournament.service';
import { EnumLabelPipe } from '../../shared/pipes/enum-label.pipe';
import { HeroComponent } from '../../shared/components/hero/hero.component';

interface DashboardStats {
  teams: number;
  tournaments: number;
  activeTournaments: number;
  players: number;
  referees: number;
  sponsors: number;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [DatePipe, EnumLabelPipe, HeroComponent],
  template: `
    <app-hero />

    <h1 class="page-title">Dashboard</h1>
    <p class="page-subtitle">
      Resumen de la liga deportiva ITM — conectado a la API REST.
    </p>

    @if (error()) {
      <div class="alert alert--error">{{ error() }}</div>
    }

    @if (loading()) {
      <div class="loading">Cargando estadísticas…</div>
    } @else if (stats()) {
      <div class="stats-grid">
        <article class="stat-card">
          <span class="stat-card__label">Equipos</span>
          <span class="stat-card__value">{{ stats()!.teams }}</span>
        </article>
        <article class="stat-card">
          <span class="stat-card__label">Torneos</span>
          <span class="stat-card__value">{{ stats()!.tournaments }}</span>
        </article>
        <article class="stat-card stat-card--accent">
          <span class="stat-card__label">Torneos activos</span>
          <span class="stat-card__value">{{ stats()!.activeTournaments }}</span>
        </article>
        <article class="stat-card">
          <span class="stat-card__label">Jugadores</span>
          <span class="stat-card__value">{{ stats()!.players }}</span>
        </article>
        <article class="stat-card">
          <span class="stat-card__label">Árbitros</span>
          <span class="stat-card__value">{{ stats()!.referees }}</span>
        </article>
        <article class="stat-card">
          <span class="stat-card__label">Patrocinadores</span>
          <span class="stat-card__value">{{ stats()!.sponsors }}</span>
        </article>
      </div>

      @if (recentTournaments().length) {
        <section class="card section">
          <h2>Últimos torneos</h2>
          <table class="data-table">
            <thead>
              <tr>
                <th>Nombre</th>
                <th>Temporada</th>
                <th>Estado</th>
                <th>Equipos</th>
                <th>Inicio</th>
              </tr>
            </thead>
            <tbody>
              @for (t of recentTournaments(); track t.id) {
                <tr>
                  <td>{{ t.name }}</td>
                  <td>{{ t.season }}</td>
                  <td>
                    <span [class]="statusClass(t.status)">
                      {{ t.status | enumLabel: 'tournamentStatus' }}
                    </span>
                  </td>
                  <td>{{ t.teamsCount }}</td>
                  <td>{{ t.startDate | date: 'mediumDate' }}</td>
                </tr>
              }
            </tbody>
          </table>
        </section>
      }
    }
  `,
  styles: `
    .stats-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(160px, 1fr));
      gap: 1rem;
      margin-bottom: 2rem;
    }

    .stat-card {
      background: var(--color-surface);
      border: 1px solid var(--color-border);
      border-radius: var(--radius);
      padding: 1.25rem;
      display: flex;
      flex-direction: column;
      gap: 0.35rem;

      &--accent {
        border-color: rgba(0, 200, 150, 0.4);
        background: linear-gradient(
          135deg,
          rgba(0, 200, 150, 0.12),
          var(--color-surface)
        );
      }
    }

    .stat-card__label {
      font-size: 0.8rem;
      color: var(--color-text-muted);
      text-transform: uppercase;
      letter-spacing: 0.04em;
    }

    .stat-card__value {
      font-family: var(--font-display);
      font-size: 2rem;
      font-weight: 700;
      color: var(--color-primary);
    }

    .section h2 {
      font-size: 1.1rem;
      margin-bottom: 1rem;
    }
  `,
})
export class DashboardComponent implements OnInit {
  private readonly teamService = inject(TeamService);
  private readonly tournamentService = inject(TournamentService);
  private readonly playerService = inject(PlayerService);
  private readonly refereeService = inject(RefereeService);
  private readonly sponsorService = inject(SponsorService);

  readonly loading = signal(true);
  readonly error = signal<string | null>(null);
  readonly stats = signal<DashboardStats | null>(null);
  readonly recentTournaments = signal<
    import('../../core/models/tournament.model').Tournament[]
  >([]);

  ngOnInit(): void {
    forkJoin({
      teams: this.teamService.getAll(),
      tournaments: this.tournamentService.getAll(),
      players: this.playerService.getAll(),
      referees: this.refereeService.getAll(),
      sponsors: this.sponsorService.getAll(),
    }).subscribe({
      next: ({ teams, tournaments, players, referees, sponsors }) => {
        this.stats.set({
          teams: teams.length,
          tournaments: tournaments.length,
          activeTournaments: tournaments.filter(
            (t) => t.status === TournamentStatus.InProgress
          ).length,
          players: players.length,
          referees: referees.length,
          sponsors: sponsors.length,
        });
        this.recentTournaments.set(
          [...tournaments]
            .sort(
              (a, b) =>
                new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()
            )
            .slice(0, 5)
        );
        this.loading.set(false);
      },
      error: () => {
        this.error.set(
          'No se pudo conectar con la API. Verifica que SportsLeague.API esté corriendo en http://localhost:5198'
        );
        this.loading.set(false);
      },
    });
  }

  statusClass(status: TournamentStatus): string {
    switch (status) {
      case TournamentStatus.Pending:
        return 'badge badge--pending';
      case TournamentStatus.InProgress:
        return 'badge badge--progress';
      default:
        return 'badge badge--finished';
    }
  }
}
