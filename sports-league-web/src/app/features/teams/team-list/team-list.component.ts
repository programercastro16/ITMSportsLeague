import { DatePipe } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Team } from '../../../core/models/team.model';
import { TeamService } from '../../../core/services/team.service';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';

@Component({
  selector: 'app-team-list',
  standalone: true,
  imports: [DatePipe, ReactiveFormsModule, PageHeaderComponent],
  template: `
    <app-page-header
      title="Equipos"
      subtitle="Gestión de clubes registrados en la liga."
    >
      <button type="button" class="btn btn--primary" (click)="toggleForm()">
        {{ showForm() ? 'Cancelar' : '+ Nuevo equipo' }}
      </button>
    </app-page-header>

    @if (error()) {
      <div class="alert alert--error">{{ error() }}</div>
    }

    @if (showForm()) {
      <form class="card form-card" [formGroup]="form" (ngSubmit)="onSubmit()">
        <h2>Nuevo equipo</h2>
        <div class="form-grid">
          <div class="form-field">
            <label for="name">Nombre</label>
            <input id="name" formControlName="name" />
          </div>
          <div class="form-field">
            <label for="city">Ciudad</label>
            <input id="city" formControlName="city" />
          </div>
          <div class="form-field">
            <label for="stadium">Estadio</label>
            <input id="stadium" formControlName="stadium" />
          </div>
          <div class="form-field">
            <label for="foundedDate">Fecha de fundación</label>
            <input id="foundedDate" type="date" formControlName="foundedDate" />
          </div>
          <div class="form-field">
            <label for="logoUrl">URL del logo (opcional)</label>
            <input id="logoUrl" formControlName="logoUrl" />
          </div>
        </div>
        <button type="submit" class="btn btn--primary" [disabled]="form.invalid || saving()">
          Guardar
        </button>
      </form>
    }

    @if (loading()) {
      <div class="loading">Cargando equipos…</div>
    } @else if (!teams().length) {
      <div class="card empty-state">
        <strong>Sin equipos</strong>
        <p>Registra el primer equipo con el botón de arriba.</p>
      </div>
    } @else {
      <div class="card table-wrap">
        <table class="data-table">
          <thead>
            <tr>
              <th>Nombre</th>
              <th>Ciudad</th>
              <th>Estadio</th>
              <th>Fundación</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            @for (team of teams(); track team.id) {
              <tr>
                <td><strong>{{ team.name }}</strong></td>
                <td>{{ team.city }}</td>
                <td>{{ team.stadium }}</td>
                <td>{{ team.foundedDate | date: 'mediumDate' }}</td>
                <td>
                  <button
                    type="button"
                    class="btn btn--danger btn--sm"
                    (click)="onDelete(team)"
                  >
                    Eliminar
                  </button>
                </td>
              </tr>
            }
          </tbody>
        </table>
      </div>
    }
  `,
  styles: `
    .form-card h2 {
      font-size: 1.1rem;
      margin-bottom: 1rem;
    }

    .form-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
      gap: 0 1rem;
    }

    .form-card {
      margin-bottom: 1.5rem;
    }

    .table-wrap {
      overflow-x: auto;
    }

    .btn--sm {
      padding: 0.35rem 0.65rem;
      font-size: 0.8rem;
    }
  `,
})
export class TeamListComponent implements OnInit {
  private readonly teamService = inject(TeamService);
  private readonly fb = inject(FormBuilder);

  readonly teams = signal<Team[]>([]);
  readonly loading = signal(true);
  readonly saving = signal(false);
  readonly error = signal<string | null>(null);
  readonly showForm = signal(false);

  readonly form = this.fb.nonNullable.group({
    name: ['', Validators.required],
    city: ['', Validators.required],
    stadium: ['', Validators.required],
    foundedDate: ['', Validators.required],
    logoUrl: [''],
  });

  ngOnInit(): void {
    this.loadTeams();
  }

  toggleForm(): void {
    this.showForm.update((v) => !v);
  }

  loadTeams(): void {
    this.loading.set(true);
    this.error.set(null);
    this.teamService.getAll().subscribe({
      next: (data) => {
        this.teams.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Error al cargar equipos.');
        this.loading.set(false);
      },
    });
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    this.saving.set(true);
    const raw = this.form.getRawValue();
    this.teamService
      .create({
        name: raw.name,
        city: raw.city,
        stadium: raw.stadium,
        foundedDate: raw.foundedDate,
        logoUrl: raw.logoUrl || null,
      })
      .subscribe({
        next: () => {
          this.form.reset();
          this.showForm.set(false);
          this.saving.set(false);
          this.loadTeams();
        },
        error: (err) => {
          this.error.set(err?.error?.message ?? 'No se pudo crear el equipo.');
          this.saving.set(false);
        },
      });
  }

  onDelete(team: Team): void {
    if (!confirm(`¿Eliminar a ${team.name}?`)) return;
    this.teamService.delete(team.id).subscribe({
      next: () => this.loadTeams(),
      error: (err) =>
        this.error.set(err?.error?.message ?? 'No se pudo eliminar.'),
    });
  }
}
