import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Team, TeamRequest } from '../models/team.model';
import { ApiService } from './api.service';

@Injectable({ providedIn: 'root' })
export class TeamService {
  private readonly api = inject(ApiService);
  private readonly resource = 'Team';

  getAll(): Observable<Team[]> {
    return this.api.get<Team[]>(this.resource);
  }

  getById(id: number): Observable<Team> {
    return this.api.get<Team>(`${this.resource}/${id}`);
  }

  create(dto: TeamRequest): Observable<Team> {
    return this.api.post<Team>(this.resource, dto);
  }

  update(id: number, dto: TeamRequest): Observable<void> {
    return this.api.put(`${this.resource}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.api.delete(`${this.resource}/${id}`);
  }
}
