# ITM Sports League — Frontend (Angular)

Panel de administración en **Angular 19** (standalone components) para la API `SportsLeague.API`.

## Estructura del proyecto

```
src/app/
├── core/           # Modelos, servicios HTTP, enums alineados con la API
├── layout/         # Shell: sidebar + topbar
├── features/       # Pantallas por dominio (dashboard, equipos, torneos…)
└── shared/         # Componentes y pipes reutilizables
```

## Requisitos

- [Node.js LTS](https://nodejs.org/) (incluye `npm` y `npx`)
- API corriendo en `http://localhost:5198`

## Cómo ejecutar

```bash
cd sports-league-web
npm install
npm start
```

Abre [http://localhost:4200](http://localhost:4200).

En otra terminal, levanta la API:

```bash
cd SportsLeague.API
dotnet run
```

## Rutas

| Ruta | Módulo |
|------|--------|
| `/` | Dashboard con resumen |
| `/equipos` | Listado + alta de equipos |
| `/torneos` | Listado de torneos |
| `/jugadores` | Listado de jugadores |
| `/arbitros` | Listado de árbitros |
| `/patrocinadores` | Listado de sponsors |

## Próximos pasos (organización)

1. Formularios CRUD para torneos, jugadores, árbitros y patrocinadores
2. Detalle de torneo: inscribir equipos, cambiar estado, ver sponsors
3. Autenticación (JWT) cuando la API lo exponga
4. Validaciones y mensajes de error unificados

## Configuración API

`src/environments/environment.development.ts`:

```ts
apiUrl: 'http://localhost:5198/api'
```

La API debe tener CORS habilitado para `http://localhost:4200` (ya configurado en `Program.cs`).
