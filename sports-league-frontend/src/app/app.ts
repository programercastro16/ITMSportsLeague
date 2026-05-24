import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ApiService } from './core/services/api.service';
import { ApiResponse } from './core/models/api-response.model';
import { Team } from './core/models/team.model';
 
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  template: `
    <h1>Sports League Frontend</h1>
    <p>Conexión a la API: {{ connectionStatus }}</p>
    @if (teams.length > 0) {
      <ul>
        @for (team of teams; track team.id) {
          <li>{{ team.name }} - {{ team.city }}</li>
        }
      </ul>
    }
    <router-outlet />
  `
})
export class App implements OnInit {
  private apiService = inject(ApiService);
  private cdr = inject(ChangeDetectorRef);
  connectionStatus = 'Verificando...';
  teams: Team[] = [];

  ngOnInit(): void {
    this.apiService.get<Team[]>('Team').subscribe({
      next: (teams) => {
        this.connectionStatus = '✔ Conectado exitosamente';
        this.teams = teams;
        this.cdr.detectChanges();    // ← Forzar actualización de la vista
        console.log('Teams:', this.teams);
      },
      error: (err) => {
        this.connectionStatus = '✘ Error de conexión';
        this.cdr.detectChanges();    // ← También en el error
        console.error('Error:', err);
      }
    });
  }
}
