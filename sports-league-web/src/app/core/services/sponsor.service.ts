import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  Sponsor,
  SponsorRequest,
  TournamentSponsor,
  TournamentSponsorRequest,
} from '../models/sponsor.model';
import { ApiService } from './api.service';

@Injectable({ providedIn: 'root' })
export class SponsorService {
  private readonly api = inject(ApiService);
  private readonly resource = 'Sponsor';

  getAll(): Observable<Sponsor[]> {
    return this.api.get<Sponsor[]>(this.resource);
  }

  getById(id: number): Observable<Sponsor> {
    return this.api.get<Sponsor>(`${this.resource}/${id}`);
  }

  create(dto: SponsorRequest): Observable<Sponsor> {
    return this.api.post<Sponsor>(this.resource, dto);
  }

  update(id: number, dto: SponsorRequest): Observable<void> {
    return this.api.put(`${this.resource}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.api.delete(`${this.resource}/${id}`);
  }

  linkToTournament(
    sponsorId: number,
    dto: TournamentSponsorRequest
  ): Observable<TournamentSponsor> {
    return this.api.post<TournamentSponsor>(
      `${this.resource}/${sponsorId}/tournaments`,
      dto
    );
  }

  getTournaments(sponsorId: number): Observable<TournamentSponsor[]> {
    return this.api.get<TournamentSponsor[]>(
      `${this.resource}/${sponsorId}/tournaments`
    );
  }

  unlinkFromTournament(sponsorId: number, tournamentId: number): Observable<void> {
    return this.api.delete(
      `${this.resource}/${sponsorId}/tournaments/${tournamentId}`
    );
  }
}
