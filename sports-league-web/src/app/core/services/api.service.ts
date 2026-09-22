import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiUrl;

  get<T>(path: string): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/${path}`);
  }

  post<T>(path: string, body: unknown): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}/${path}`, body);
  }

  put(path: string, body: unknown): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${path}`, body);
  }

  patch(path: string, body: unknown): Observable<void> {
    return this.http.patch<void>(`${this.baseUrl}/${path}`, body);
  }

  delete(path: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${path}`);
  }
}
