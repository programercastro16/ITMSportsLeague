import { Component, inject, OnInit, signal } from '@angular/core';
import { Sponsor } from '../../../core/models/sponsor.model';
import { SponsorService } from '../../../core/services/sponsor.service';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';
import { EnumLabelPipe } from '../../../shared/pipes/enum-label.pipe';

@Component({
  selector: 'app-sponsor-list',
  standalone: true,
  imports: [PageHeaderComponent, EnumLabelPipe],
  template: `
    <app-page-header
      title="Patrocinadores"
      subtitle="Marcas y contratos con torneos."
    />

    @if (error()) {
      <div class="alert alert--error">{{ error() }}</div>
    }

    @if (loading()) {
      <div class="loading">Cargando patrocinadores…</div>
    } @else if (!sponsors().length) {
      <div class="card empty-state">
        <strong>Sin patrocinadores</strong>
        <p>Registra sponsors y enlázalos a torneos.</p>
      </div>
    } @else {
      <div class="card table-wrap">
        <table class="data-table">
          <thead>
            <tr>
              <th>Nombre</th>
              <th>Categoría</th>
              <th>Email</th>
              <th>Teléfono</th>
              <th>Sitio web</th>
            </tr>
          </thead>
          <tbody>
            @for (s of sponsors(); track s.id) {
              <tr>
                <td><strong>{{ s.name }}</strong></td>
                <td>{{ s.category | enumLabel: 'sponsorCategory' }}</td>
                <td>{{ s.contactEmail }}</td>
                <td>{{ s.phone ?? '—' }}</td>
                <td>
                  @if (s.websiteUrl) {
                    <a [href]="s.websiteUrl" target="_blank" rel="noopener">
                      {{ s.websiteUrl }}
                    </a>
                  } @else {
                    —
                  }
                </td>
              </tr>
            }
          </tbody>
        </table>
      </div>
    }
  `,
  styles: `
    .table-wrap {
      overflow-x: auto;
    }

    td a {
      word-break: break-all;
    }
  `,
})
export class SponsorListComponent implements OnInit {
  private readonly sponsorService = inject(SponsorService);

  readonly sponsors = signal<Sponsor[]>([]);
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);

  ngOnInit(): void {
    this.sponsorService.getAll().subscribe({
      next: (data) => {
        this.sponsors.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Error al cargar patrocinadores.');
        this.loading.set(false);
      },
    });
  }
}
