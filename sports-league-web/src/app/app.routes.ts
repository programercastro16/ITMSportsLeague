import { Routes } from '@angular/router';
import { ShellComponent } from './layout/shell/shell.component';

export const routes: Routes = [
  {
    path: '',
    component: ShellComponent,
    children: [
      {
        path: '',
        loadComponent: () =>
          import('./features/dashboard/dashboard.component').then(
            (m) => m.DashboardComponent
          ),
      },
      {
        path: 'equipos',
        loadComponent: () =>
          import('./features/teams/team-list/team-list.component').then(
            (m) => m.TeamListComponent
          ),
      },
      {
        path: 'torneos',
        loadComponent: () =>
          import('./features/tournaments/tournament-list/tournament-list.component').then(
            (m) => m.TournamentListComponent
          ),
      },
      {
        path: 'jugadores',
        loadComponent: () =>
          import('./features/players/player-list/player-list.component').then(
            (m) => m.PlayerListComponent
          ),
      },
      {
        path: 'arbitros',
        loadComponent: () =>
          import('./features/referees/referee-list/referee-list.component').then(
            (m) => m.RefereeListComponent
          ),
      },
      {
        path: 'patrocinadores',
        loadComponent: () =>
          import('./features/sponsors/sponsor-list/sponsor-list.component').then(
            (m) => m.SponsorListComponent
          ),
      },
    ],
  },
  { path: '**', redirectTo: '' },
];
