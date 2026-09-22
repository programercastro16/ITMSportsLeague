import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Team } from '../models/team.model';
import {
  RegisterTeamRequest,
  Tournament,
  TournamentRequest,
  UpdateStatusRequest,
} from '../models/tournament.model';
import { ApiService } from './api.service';

@Injectable({ providedIn: 'root' })
export class TournamentService {
  private readonly api = inject(ApiService);
  private readonly resource = 'Tournament';

  getAll(): Observable<Tournament[]> {
    return this.api.get<Tournament[]>(this.resource);
  }

  getById(id: number): Observable<Tournament> {
    return this.api.get<Tournament>(`${this.resource}/${id}`);
  }

  create(dto: TournamentRequest): Observable<Tournament> {
    return this.api.post<Tournament>(this.resource, dto);
  }

  update(id: number, dto: TournamentRequest): Observable<void> {
    return this.api.put(`${this.resource}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.api.delete(`${this.resource}/${id}`);
  }

  updateStatus(id: number, dto: UpdateStatusRequest): Observable<void> {
    return this.api.patch(`${this.resource}/${id}/status`, dto);
  }

  registerTeam(tournamentId: number, dto: RegisterTeamRequest): Observable<{ message: string }> {
    return this.api.post<{ message: string }>(
      `${this.resource}/${tournamentId}/teams`,
      dto
    );
  }

  getTeams(tournamentId: number): Observable<Team[]> {
    return this.api.get<Team[]>(`${this.resource}/${tournamentId}/teams`);
  }
}
