import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Referee, RefereeRequest } from '../models/referee.model';
import { ApiService } from './api.service';

@Injectable({ providedIn: 'root' })
export class RefereeService {
  private readonly api = inject(ApiService);
  private readonly resource = 'Referee';

  getAll(): Observable<Referee[]> {
    return this.api.get<Referee[]>(this.resource);
  }

  getById(id: number): Observable<Referee> {
    return this.api.get<Referee>(`${this.resource}/${id}`);
  }

  create(dto: RefereeRequest): Observable<Referee> {
    return this.api.post<Referee>(this.resource, dto);
  }

  update(id: number, dto: RefereeRequest): Observable<void> {
    return this.api.put(`${this.resource}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.api.delete(`${this.resource}/${id}`);
  }
}
