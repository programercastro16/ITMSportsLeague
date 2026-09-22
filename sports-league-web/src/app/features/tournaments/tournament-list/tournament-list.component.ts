import { DatePipe } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { TournamentStatus } from '../../../core/models/enums';
import { Tournament } from '../../../core/models/tournament.model';
import { TournamentService } from '../../../core/services/tournament.service';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';
import { EnumLabelPipe } from '../../../shared/pipes/enum-label.pipe';

@Component({
  selector: 'app-tournament-list',
  standalone: true,
  imports: [DatePipe, PageHeaderComponent, EnumLabelPipe],
  template: `
    <app-page-header
      title="Torneos"
      subtitle="Temporadas y competencias de la liga."
    />

    @if (error()) {
      <div class="alert alert--error">{{ error() }}</div>
    }

    @if (loading()) {
      <div class="loading">Cargando torneos…</div>
    } @else if (!tournaments().length) {
      <div class="card empty-state">
        <strong>Sin torneos</strong>
        <p>Los torneos se crean desde Swagger o el formulario (próximo paso).</p>
      </div>
    } @else {
      <div class="card table-wrap">
        <table class="data-table">
          <thead>
            <tr>
              <th>Nombre</th>
              <th>Temporada</th>
              <th>Estado</th>
              <th>Equipos</th>
              <th>Inicio</th>
              <th>Fin</th>
            </tr>
          </thead>
          <tbody>
            @for (t of tournaments(); track t.id) {
              <tr>
                <td><strong>{{ t.name }}</strong></td>
                <td>{{ t.season }}</td>
                <td>
                  <span [class]="statusClass(t.status)">
                    {{ t.status | enumLabel: 'tournamentStatus' }}
                  </span>
                </td>
                <td>{{ t.teamsCount }}</td>
                <td>{{ t.startDate | date: 'mediumDate' }}</td>
                <td>{{ t.endDate | date: 'mediumDate' }}</td>
              </tr>
            }
          </tbody>
        </table>
      </div>
    }
  `,
  styles: `.table-wrap { overflow-x: auto; }`,
})
export class TournamentListComponent implements OnInit {
  private readonly tournamentService = inject(TournamentService);

  readonly tournaments = signal<Tournament[]>([]);
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);

  ngOnInit(): void {
    this.tournamentService.getAll().subscribe({
      next: (data) => {
        this.tournaments.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Error al cargar torneos.');
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
