# ITM Sports League

Sistema de gestión de liga deportiva — backend .NET y frontend Angular.

## Estructura del repositorio

| Carpeta | Descripción |
|---------|-------------|
| `SportsLeague.API` | API REST (ASP.NET Core) |
| `SportsLeague.Domain` | Entidades, enums, servicios de dominio |
| `SportsLeague.DataAccess` | EF Core, repositorios, migraciones |
| `sports-league-web` | Frontend Angular 19 |

## Inicio rápido

### 1. Base de datos y API

```bash
cd SportsLeague.API
dotnet ef database update --project ../SportsLeague.DataAccess
dotnet run
```

Swagger: [http://localhost:5198/swagger](http://localhost:5198/swagger)

### 2. Frontend Angular

Requiere Node.js LTS instalado.

```bash
cd sports-league-web
npm install
npm start
```

App: [http://localhost:4200](http://localhost:4200)

## Dominios de la API

- `api/Team` — Equipos
- `api/Tournament` — Torneos (estado, inscripción de equipos)
- `api/Player` — Jugadores
- `api/Referee` — Árbitros
- `api/Sponsor` — Patrocinadores y vínculos con torneos

## Documentación adicional

Ver `sports-league-web/README.md` para la arquitectura del frontend y el roadmap de pantallas.
