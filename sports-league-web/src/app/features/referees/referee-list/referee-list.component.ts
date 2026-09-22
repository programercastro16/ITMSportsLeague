import { Component, inject, OnInit, signal } from '@angular/core';
import { Referee } from '../../../core/models/referee.model';
import { RefereeService } from '../../../core/services/referee.service';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';

@Component({
  selector: 'app-referee-list',
  standalone: true,
  imports: [PageHeaderComponent],
  template: `
    <app-page-header
      title="Árbitros"
      subtitle="Cuerpo arbitral disponible para partidos."
    />

    @if (error()) {
      <div class="alert alert--error">{{ error() }}</div>
    }

    @if (loading()) {
      <div class="loading">Cargando árbitros…</div>
    } @else if (!referees().length) {
      <div class="card empty-state">
        <strong>Sin árbitros</strong>
        <p>Agrega árbitros al sistema.</p>
      </div>
    } @else {
      <div class="card table-wrap">
        <table class="data-table">
          <thead>
            <tr>
              <th>Nombre</th>
              <th>Nacionalidad</th>
            </tr>
          </thead>
          <tbody>
            @for (r of referees(); track r.id) {
              <tr>
                <td><strong>{{ r.firstName }} {{ r.lastName }}</strong></td>
                <td>{{ r.nationality }}</td>
              </tr>
            }
          </tbody>
        </table>
      </div>
    }
  `,
  styles: `.table-wrap { overflow-x: auto; }`,
})
export class RefereeListComponent implements OnInit {
  private readonly refereeService = inject(RefereeService);

  readonly referees = signal<Referee[]>([]);
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);

  ngOnInit(): void {
    this.refereeService.getAll().subscribe({
      next: (data) => {
        this.referees.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Error al cargar árbitros.');
        this.loading.set(false);
      },
    });
  }
}
