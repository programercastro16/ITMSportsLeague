import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

interface NavItem {
  path: string;
  label: string;
  icon: string;
}

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  template: `
    <div class="shell">
      <aside class="sidebar">
        <div class="sidebar__brand">
          <span class="sidebar__logo">⚽</span>
          <div>
            <strong>ITM Sports</strong>
            <small>League Admin</small>
          </div>
        </div>

        <nav class="sidebar__nav">
          @for (item of navItems; track item.path) {
            <a
              [routerLink]="item.path"
              routerLinkActive="active"
              [routerLinkActiveOptions]="{ exact: item.path === '/' }"
              class="sidebar__link"
            >
              <span class="sidebar__icon">{{ item.icon }}</span>
              {{ item.label }}
            </a>
          }
        </nav>

        <footer class="sidebar__footer">
          <span>API: localhost:5198</span>
        </footer>
      </aside>

      <div class="shell__main">
        <header class="topbar">
          <span class="topbar__tag">Panel de administración</span>
        </header>
        <main class="content">
          <router-outlet />
        </main>
      </div>
    </div>
  `,
  styles: `
    .shell {
      display: flex;
      min-height: 100vh;
    }

    .sidebar {
      width: var(--sidebar-width);
      background: var(--color-bg-elevated);
      border-right: 1px solid var(--color-border);
      display: flex;
      flex-direction: column;
      padding: 1.25rem 0;
      position: fixed;
      top: 0;
      bottom: 0;
      left: 0;
      z-index: 10;
    }

    .sidebar__brand {
      display: flex;
      align-items: center;
      gap: 0.75rem;
      padding: 0 1.25rem 1.5rem;
      border-bottom: 1px solid var(--color-border);
      margin-bottom: 1rem;

      strong {
        display: block;
        font-family: var(--font-display);
        font-size: 1.05rem;
      }

      small {
        color: var(--color-text-muted);
        font-size: 0.75rem;
      }
    }

    .sidebar__logo {
      font-size: 1.75rem;
      line-height: 1;
    }

    .sidebar__nav {
      flex: 1;
      display: flex;
      flex-direction: column;
      gap: 0.25rem;
      padding: 0 0.75rem;
    }

    .sidebar__link {
      display: flex;
      align-items: center;
      gap: 0.65rem;
      padding: 0.65rem 0.85rem;
      border-radius: 8px;
      color: var(--color-text-muted);
      font-weight: 500;
      font-size: 0.9rem;
      transition: background 0.15s, color 0.15s;

      &:hover {
        background: rgba(255, 255, 255, 0.05);
        color: var(--color-text);
      }

      &.active {
        background: rgba(0, 200, 150, 0.15);
        color: var(--color-primary);
      }
    }

    .sidebar__icon {
      width: 1.25rem;
      text-align: center;
    }

    .sidebar__footer {
      padding: 1rem 1.25rem 0;
      font-size: 0.7rem;
      color: var(--color-text-muted);
      border-top: 1px solid var(--color-border);
      margin-top: auto;
      padding-top: 1rem;
    }

    .shell__main {
      flex: 1;
      margin-left: var(--sidebar-width);
      display: flex;
      flex-direction: column;
      min-width: 0;
    }

    .topbar {
      height: var(--header-height);
      border-bottom: 1px solid var(--color-border);
      display: flex;
      align-items: center;
      padding: 0 2rem;
      background: rgba(11, 18, 32, 0.85);
      backdrop-filter: blur(8px);
      position: sticky;
      top: 0;
      z-index: 5;
    }

    .topbar__tag {
      font-size: 0.8rem;
      color: var(--color-text-muted);
      text-transform: uppercase;
      letter-spacing: 0.06em;
    }

    .content {
      padding: 2rem;
      flex: 1;
    }

    @media (max-width: 900px) {
      .sidebar {
        width: 72px;
      }

      .sidebar__brand div,
      .sidebar__link span:not(.sidebar__icon),
      .sidebar__footer {
        display: none;
      }

      .shell__main {
        margin-left: 72px;
      }
    }
  `,
})
export class ShellComponent {
  readonly navItems: NavItem[] = [
    { path: '/', label: 'Dashboard', icon: '📊' },
    { path: '/equipos', label: 'Equipos', icon: '🏟️' },
    { path: '/torneos', label: 'Torneos', icon: '🏆' },
    { path: '/jugadores', label: 'Jugadores', icon: '👤' },
    { path: '/arbitros', label: 'Árbitros', icon: '🟨' },
    { path: '/patrocinadores', label: 'Patrocinadores', icon: '🤝' },
  ];
}
