import { DatePipe } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { Player } from '../../../core/models/player.model';
import { PlayerService } from '../../../core/services/player.service';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';
import { EnumLabelPipe } from '../../../shared/pipes/enum-label.pipe';

@Component({
  selector: 'app-player-list',
  standalone: true,
  imports: [DatePipe, PageHeaderComponent, EnumLabelPipe],
  template: `
    <app-page-header
      title="Jugadores"
      subtitle="Plantillas y dorsales por equipo."
    />

    @if (error()) {
      <div class="alert alert--error">{{ error() }}</div>
    }

    @if (loading()) {
      <div class="loading">Cargando jugadores…</div>
    } @else if (!players().length) {
      <div class="card empty-state">
        <strong>Sin jugadores</strong>
        <p>Registra jugadores asociados a un equipo.</p>
      </div>
    } @else {
      <div class="card table-wrap">
        <table class="data-table">
          <thead>
            <tr>
              <th>#</th>
              <th>Nombre</th>
              <th>Posición</th>
              <th>Equipo</th>
              <th>Nacimiento</th>
            </tr>
          </thead>
          <tbody>
            @for (p of players(); track p.id) {
              <tr>
                <td>{{ p.number }}</td>
                <td><strong>{{ p.firstName }} {{ p.lastName }}</strong></td>
                <td>{{ p.position | enumLabel: 'playerPosition' }}</td>
                <td>{{ p.teamName }}</td>
                <td>{{ p.birthDate | date: 'mediumDate' }}</td>
              </tr>
            }
          </tbody>
        </table>
      </div>
    }
  `,
  styles: `.table-wrap { overflow-x: auto; }`,
})
export class PlayerListComponent implements OnInit {
  private readonly playerService = inject(PlayerService);

  readonly players = signal<Player[]>([]);
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);

  ngOnInit(): void {
    this.playerService.getAll().subscribe({
      next: (data) => {
        this.players.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Error al cargar jugadores.');
        this.loading.set(false);
      },
    });
  }
}
