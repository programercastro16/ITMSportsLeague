import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Player, PlayerRequest } from '../models/player.model';
import { ApiService } from './api.service';

@Injectable({ providedIn: 'root' })
export class PlayerService {
  private readonly api = inject(ApiService);
  private readonly resource = 'Player';

  getAll(): Observable<Player[]> {
    return this.api.get<Player[]>(this.resource);
  }

  getById(id: number): Observable<Player> {
    return this.api.get<Player>(`${this.resource}/${id}`);
  }

  create(dto: PlayerRequest): Observable<Player> {
    return this.api.post<Player>(this.resource, dto);
  }

  update(id: number, dto: PlayerRequest): Observable<void> {
    return this.api.put(`${this.resource}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.api.delete(`${this.resource}/${id}`);
  }
}
