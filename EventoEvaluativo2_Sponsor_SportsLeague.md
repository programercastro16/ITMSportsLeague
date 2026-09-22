# Evento Evaluativo 2 – Implementación de Sponsor en SportsLeague

Este documento explica **paso a paso** qué archivos crear/modificar, **en qué carpeta va cada uno** y **qué código pegar**, para que puedas replicar la solución en otro proyecto siguiendo la misma estructura que ya tienes.

> Nota: Las rutas están dadas **desde la raíz de la solución** `ITMSportsLeague-master`.

---

## 1. Dominio (SportsLeague.Domain)

### 1.1. Enum `SponsorCategory`

- **Ruta**: `SportsLeague.Domain/Enums/SponsorCategory.cs`
- **Acción**: Crear archivo nuevo `SponsorCategory.cs` con el siguiente código:

```csharp
namespace SportsLeague.Domain.Enums
{
    public enum SponsorCategory
    {
        Main = 0, // Patrocinador principal
        Gold = 1, // Oro
        Silver = 2, // Plata
        Bronze = 3 // Bronce
    }
}
```

---

### 1.2. Entidad `Sponsor`

- **Ruta**: `SportsLeague.Domain/Entities/Sponsor.cs`
- **Acción**: Crear archivo nuevo `Sponsor.cs` con:

```csharp
using System.Collections.Generic;
using SportsLeague.Domain.Enums;

namespace SportsLeague.Domain.Entities
{
    public class Sponsor : AuditBase
    {
        public string Name { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? WebsiteUrl { get; set; }
        public SponsorCategory Category { get; set; }

        // Navigation Properties
        public ICollection<TournamentSponsor> TournamentSponsors { get; set; } = new List<TournamentSponsor>();
    }
}
```

---

### 1.3. Entidad intermedia `TournamentSponsor` (N:M)

- **Ruta**: `SportsLeague.Domain/Entities/TournamentSponsor.cs`
- **Acción**: Crear archivo nuevo `TournamentSponsor.cs`:

```csharp
namespace SportsLeague.Domain.Entities
{
    public class TournamentSponsor : AuditBase
    {
        public int TournamentId { get; set; }
        public int SponsorId { get; set; }
        public decimal ContractAmount { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Tournament Tournament { get; set; } = null!;
        public Sponsor Sponsor { get; set; } = null!;
    }
}
```

---

### 1.4. Agregar colección de `TournamentSponsor` en `Tournament`

- **Ruta**: `SportsLeague.Domain/Entities/Tournament.cs`
- **Acción**: Abrir `Tournament.cs` y **agregar la colección** de `TournamentSponsor` dentro de la clase.

Código esperado de la clase (parte relevante):

```csharp
using SportsLeague.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.Domain.Entities
{
    public class Tournament : AuditBase
    {
        public string Name { get; set; } = string.Empty;
        public string Season { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TournamentStatus Status { get; set; } = TournamentStatus.Pending;

        // Navigation Properties
        public ICollection<TournamentTeam> TournamentTeams { get; set; } = new List<TournamentTeam>();
        public ICollection<TournamentSponsor> TournamentSponsors { get; set; } = new List<TournamentSponsor>();
    }
}
```

---

### 1.5. Interfaces de Repositorio

#### 1.5.1. `ISponsorRepository`

- **Ruta**: `SportsLeague.Domain/Interfaces/Repositories/ISponsorRepository.cs`
- **Acción**: Crear archivo nuevo:

```csharp
using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface ISponsorRepository : IGenericRepository<Sponsor>
    {
        Task<bool> ExistsByNameAsync(string name);
    }
}
```

#### 1.5.2. `ITournamentSponsorRepository`

- **Ruta**: `SportsLeague.Domain/Interfaces/Repositories/ITournamentSponsorRepository.cs`
- **Acción**: Crear archivo nuevo:

```csharp
using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface ITournamentSponsorRepository : IGenericRepository<TournamentSponsor>
    {
        Task<TournamentSponsor?> GetBySponsorAndTournamentAsync(int sponsorId, int tournamentId);
        Task<IEnumerable<TournamentSponsor>> GetBySponsorIdAsync(int sponsorId);
        Task<IEnumerable<TournamentSponsor>> GetByTournamentIdAsync(int tournamentId);
    }
}
```

---

### 1.6. Interface de Servicio `ISponsorService`

- **Ruta**: `SportsLeague.Domain/Interfaces/Services/ISponsorService.cs`
- **Acción**: Crear archivo nuevo:

```csharp
using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Services
{
    public interface ISponsorService
    {
        Task<IEnumerable<Sponsor>> GetAllAsync();
        Task<Sponsor?> GetByIdAsync(int id);
        Task<Sponsor> CreateAsync(Sponsor sponsor);
        Task<Sponsor> UpdateAsync(int id, Sponsor sponsor);
        Task DeleteAsync(int id);

        Task<TournamentSponsor> LinkSponsorToTournamentAsync(int sponsorId, int tournamentId, decimal contractAmount);
        Task<IEnumerable<TournamentSponsor>> GetTournamentsBySponsorAsync(int sponsorId);
        Task UnlinkSponsorFromTournamentAsync(int sponsorId, int tournamentId);
    }
}
```

> Nota: Si en tu versión devuelves `Task` en `UpdateAsync` como en otros servicios, puedes ajustar la firma para que sea consistente (en el código implementado se usa `Task` sin valor de retorno).

---

### 1.7. Servicio de Dominio `SponsorService`

- **Ruta**: `SportsLeague.Domain/Services/SponsorService.cs`
- **Acción**: Crear archivo nuevo con:

```csharp
using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;
using System.Net.Mail;

namespace SportsLeague.Domain.Services
{
    public class SponsorService : ISponsorService
    {
        private readonly ISponsorRepository _sponsorRepository;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly ITournamentSponsorRepository _tournamentSponsorRepository;
        private readonly ILogger<SponsorService> _logger;

        public SponsorService(
            ISponsorRepository sponsorRepository,
            ITournamentRepository tournamentRepository,
            ITournamentSponsorRepository tournamentSponsorRepository,
            ILogger<SponsorService> logger)
        {
            _sponsorRepository = sponsorRepository;
            _tournamentRepository = tournamentRepository;
            _tournamentSponsorRepository = tournamentSponsorRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Sponsor>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all sponsors");
            return await _sponsorRepository.GetAllAsync();
        }

        public async Task<Sponsor?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving sponsor with ID: {SponsorId}", id);
            return await _sponsorRepository.GetByIdAsync(id);
        }

        public async Task<Sponsor> CreateAsync(Sponsor sponsor)
        {
            ValidateContactEmailFormat(sponsor.ContactEmail);

            var exists = await _sponsorRepository.ExistsByNameAsync(sponsor.Name);
            if (exists)
            {
                _logger.LogWarning("Sponsor with name '{SponsorName}' already exists", sponsor.Name);
                throw new InvalidOperationException($"Ya existe un sponsor con el nombre '{sponsor.Name}'");
            }

            _logger.LogInformation("Creating sponsor: {SponsorName}", sponsor.Name);
            return await _sponsorRepository.CreateAsync(sponsor);
        }

        public async Task<Sponsor> UpdateAsync(int id, Sponsor sponsor)
        {
            var existing = await _sponsorRepository.GetByIdAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("Sponsor with ID {SponsorId} not found for update", id);
                throw new KeyNotFoundException($"No se encontró el sponsor con ID {id}");
            }

            ValidateContactEmailFormat(sponsor.ContactEmail);

            if (existing.Name != sponsor.Name)
            {
                var nameExists = await _sponsorRepository.ExistsByNameAsync(sponsor.Name);
                if (nameExists)
                {
                    throw new InvalidOperationException($"Ya existe un sponsor con el nombre '{sponsor.Name}'");
                }
            }

            existing.Name = sponsor.Name;
            existing.ContactEmail = sponsor.ContactEmail;
            existing.Phone = sponsor.Phone;
            existing.WebsiteUrl = sponsor.WebsiteUrl;
            existing.Category = sponsor.Category;

            _logger.LogInformation("Updating sponsor with ID: {SponsorId}", id);
            await _sponsorRepository.UpdateAsync(existing);

            return existing;
        }

        public async Task DeleteAsync(int id)
        {
            var exists = await _sponsorRepository.ExistsAsync(id);
            if (!exists)
            {
                _logger.LogWarning("Sponsor with ID {SponsorId} not found for deletion", id);
                throw new KeyNotFoundException($"No se encontró el sponsor con ID {id}");
            }

            _logger.LogInformation("Deleting sponsor with ID: {SponsorId}", id);
            await _sponsorRepository.DeleteAsync(id);
        }

        public async Task<TournamentSponsor> LinkSponsorToTournamentAsync(
            int sponsorId,
            int tournamentId,
            decimal contractAmount)
        {
            if (contractAmount <= 0)
                throw new InvalidOperationException("ContractAmount debe ser mayor a 0");

            var sponsor = await _sponsorRepository.GetByIdAsync(sponsorId);
            if (sponsor == null)
                throw new KeyNotFoundException($"No se encontró el sponsor con ID {sponsorId}");

            var tournament = await _tournamentRepository.GetByIdAsync(tournamentId);
            if (tournament == null)
                throw new KeyNotFoundException($"No se encontró el torneo con ID {tournamentId}");

            var existingLink = await _tournamentSponsorRepository
                .GetBySponsorAndTournamentAsync(sponsorId, tournamentId);

            if (existingLink != null)
                throw new InvalidOperationException("El sponsor ya está vinculado a este torneo");

            var link = new TournamentSponsor
            {
                TournamentId = tournamentId,
                SponsorId = sponsorId,
                ContractAmount = contractAmount,
                JoinedAt = DateTime.UtcNow
            };

            _logger.LogInformation(
                "Linking sponsor {SponsorId} to tournament {TournamentId}",
                sponsorId, tournamentId);

            await _tournamentSponsorRepository.CreateAsync(link);

            // Recargar con navegación para el response (nombres)
            var created = await _tournamentSponsorRepository
                .GetBySponsorAndTournamentAsync(sponsorId, tournamentId);

            return created ?? link;
        }

        public async Task<IEnumerable<TournamentSponsor>> GetTournamentsBySponsorAsync(int sponsorId)
        {
            var sponsor = await _sponsorRepository.GetByIdAsync(sponsorId);
            if (sponsor == null)
                throw new KeyNotFoundException($"No se encontró el sponsor con ID {sponsorId}");

            return await _tournamentSponsorRepository.GetBySponsorIdAsync(sponsorId);
        }

        public async Task UnlinkSponsorFromTournamentAsync(int sponsorId, int tournamentId)
        {
            var sponsor = await _sponsorRepository.GetByIdAsync(sponsorId);
            if (sponsor == null)
                throw new KeyNotFoundException($"No se encontró el sponsor con ID {sponsorId}");

            var tournament = await _tournamentRepository.GetByIdAsync(tournamentId);
            if (tournament == null)
                throw new KeyNotFoundException($"No se encontró el torneo con ID {tournamentId}");

            var existingLink = await _tournamentSponsorRepository
                .GetBySponsorAndTournamentAsync(sponsorId, tournamentId);

            if (existingLink == null)
                throw new KeyNotFoundException("La vinculación sponsor-torneo no existe");

            await _tournamentSponsorRepository.DeleteAsync(existingLink.Id);
        }

        private static void ValidateContactEmailFormat(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new InvalidOperationException("ContactEmail debe ser un formato válido");

            try
            {
                var mail = new MailAddress(email);
                if (string.IsNullOrWhiteSpace(mail.Address))
                    throw new InvalidOperationException("ContactEmail debe ser un formato válido");
            }
            catch (FormatException)
            {
                throw new InvalidOperationException("ContactEmail debe ser un formato válido");
            }
        }
    }
}
```

---

## 2. Capa de Acceso a Datos (SportsLeague.DataAccess)

### 2.1. Repositorio `SponsorRepository`

- **Ruta**: `SportsLeague.DataAccess/Repositories/SponsorRepository.cs`
- **Acción**: Crear archivo nuevo:

```csharp
using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories
{
    public class SponsorRepository : GenericRepository<Sponsor>, ISponsorRepository
    {
        public SponsorRepository(LeagueDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _dbSet.AnyAsync(s => s.Name.ToLower() == name.ToLower());
        }
    }
}
```

---

### 2.2. Repositorio `TournamentSponsorRepository`

- **Ruta**: `SportsLeague.DataAccess/Repositories/TournamentSponsorRepository.cs`
- **Acción**: Crear archivo nuevo:

```csharp
using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories
{
    public class TournamentSponsorRepository : GenericRepository<TournamentSponsor>, ITournamentSponsorRepository
    {
        public TournamentSponsorRepository(LeagueDbContext context) : base(context)
        {
        }

        public async Task<TournamentSponsor?> GetBySponsorAndTournamentAsync(int sponsorId, int tournamentId)
        {
            return await _dbSet
                .Include(ts => ts.Tournament)
                .Include(ts => ts.Sponsor)
                .FirstOrDefaultAsync(ts => ts.SponsorId == sponsorId && ts.TournamentId == tournamentId);
        }

        public async Task<IEnumerable<TournamentSponsor>> GetBySponsorIdAsync(int sponsorId)
        {
            return await _dbSet
                .Where(ts => ts.SponsorId == sponsorId)
                .Include(ts => ts.Tournament)
                .Include(ts => ts.Sponsor)
                .ToListAsync();
        }

        public async Task<IEnumerable<TournamentSponsor>> GetByTournamentIdAsync(int tournamentId)
        {
            return await _dbSet
                .Where(ts => ts.TournamentId == tournamentId)
                .Include(ts => ts.Tournament)
                .Include(ts => ts.Sponsor)
                .ToListAsync();
        }
    }
}
```

---

### 2.3. Configuración en `LeagueDbContext`

- **Ruta**: `SportsLeague.DataAccess/Context/LeagueDbContext.cs`
- **Acción 1**: Agregar los nuevos `DbSet`:

```csharp
public DbSet<Team> Teams => Set<Team>();
public DbSet<Player> Players => Set<Player>();
public DbSet<Referee> Referees => Set<Referee>(); 
public DbSet<Tournament> Tournaments => Set<Tournament>(); 
public DbSet<TournamentTeam> TournamentTeams => Set<TournamentTeam>(); 
public DbSet<Sponsor> Sponsors => Set<Sponsor>();
public DbSet<TournamentSponsor> TournamentSponsors => Set<TournamentSponsor>();
```

- **Acción 2**: Al final de `OnModelCreating`, agregar la configuración de Sponsor:

```csharp
// ── Sponsor Configuration ──

modelBuilder.Entity<Sponsor>(entity =>
{
    entity.HasKey(s => s.Id);

    entity.Property(s => s.Name)
          .IsRequired()
          .HasMaxLength(150);

    entity.Property(s => s.ContactEmail)
          .IsRequired()
          .HasMaxLength(200);

    entity.Property(s => s.Phone)
          .HasMaxLength(50);

    entity.Property(s => s.WebsiteUrl)
          .HasMaxLength(250);

    entity.Property(s => s.Category)
          .IsRequired();

    entity.Property(s => s.CreatedAt)
          .IsRequired();

    entity.Property(s => s.UpdatedAt)
          .IsRequired(false);

    entity.HasIndex(s => s.Name)
          .IsUnique();
});
```

- **Acción 3**: A continuación, agregar la configuración de `TournamentSponsor`:

```csharp
// ── TournamentSponsor Configuration (N:M) ──

modelBuilder.Entity<TournamentSponsor>(entity =>
{
    entity.HasKey(ts => ts.Id);

    entity.Property(ts => ts.ContractAmount)
          .IsRequired();

    entity.Property(ts => ts.JoinedAt)
          .IsRequired();

    entity.Property(ts => ts.CreatedAt)
          .IsRequired();

    entity.Property(ts => ts.UpdatedAt)
          .IsRequired(false);

    entity.HasOne(ts => ts.Tournament)
          .WithMany(t => t.TournamentSponsors)
          .HasForeignKey(ts => ts.TournamentId)
          .OnDelete(DeleteBehavior.Cascade);

    entity.HasOne(ts => ts.Sponsor)
          .WithMany(s => s.TournamentSponsors)
          .HasForeignKey(ts => ts.SponsorId)
          .OnDelete(DeleteBehavior.Cascade);

    entity.HasIndex(ts => new { ts.TournamentId, ts.SponsorId })
          .IsUnique();
});
```

---

## 3. Capa API (SportsLeague.API)

### 3.1. DTOs de Sponsor

#### 3.1.1. `SponsorRequestDTO`

- **Ruta**: `SportsLeague.API/DTOs/Request/SponsorRequestDTO.cs`
- **Acción**: Crear archivo nuevo:

```csharp
using SportsLeague.Domain.Enums;

namespace SportsLeague.API.DTOs.Request
{
    public class SponsorRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? WebsiteUrl { get; set; }
        public SponsorCategory Category { get; set; }
    }
}
```

#### 3.1.2. `SponsorResponseDTO`

- **Ruta**: `SportsLeague.API/DTOs/Response/SponsorResponseDTO.cs`
- **Acción**: Crear archivo nuevo:

```csharp
using SportsLeague.Domain.Enums;

namespace SportsLeague.API.DTOs.Response
{
    public class SponsorResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? WebsiteUrl { get; set; }
        public SponsorCategory Category { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
```

---

### 3.2. DTOs de TournamentSponsor

#### 3.2.1. `TournamentSponsorRequestDTO`

- **Ruta**: `SportsLeague.API/DTOs/Request/TournamentSponsorRequestDTO.cs`
- **Acción**: Crear archivo nuevo:

```csharp
namespace SportsLeague.API.DTOs.Request
{
    public class TournamentSponsorRequestDTO
    {
        public int TournamentId { get; set; }
        public decimal ContractAmount { get; set; }
    }
}
```

#### 3.2.2. `TournamentSponsorResponseDTO`

- **Ruta**: `SportsLeague.API/DTOs/Response/TournamentSponsorResponseDTO.cs`
- **Acción**: Crear archivo nuevo:

```csharp
namespace SportsLeague.API.DTOs.Response
{
    public class TournamentSponsorResponseDTO
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public string TournamentName { get; set; } = string.Empty;

        public int SponsorId { get; set; }
        public string SponsorName { get; set; } = string.Empty;

        public decimal ContractAmount { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
```

---

### 3.3. Mapping con AutoMapper

- **Ruta**: `SportsLeague.API/Mappings/MappingProfile.cs`
- **Acción**: Abrir el archivo y, debajo de los mapeos de Tournament, agregar:

```csharp
// Sponsor mappings
CreateMap<SponsorRequestDTO, Sponsor>();
CreateMap<Sponsor, SponsorResponseDTO>();

CreateMap<TournamentSponsorRequestDTO, TournamentSponsor>();
CreateMap<TournamentSponsor, TournamentSponsorResponseDTO>()
    .ForMember(dest => dest.TournamentName, opt => opt.MapFrom(src => src.Tournament.Name))
    .ForMember(dest => dest.SponsorName, opt => opt.MapFrom(src => src.Sponsor.Name));
```

El archivo completo debe verse similar a:

```csharp
using AutoMapper;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
 
namespace SportsLeague.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Team mappings
        CreateMap<TeamRequestDTO, Team>();
        CreateMap<Team, TeamResponseDTO>();

        // Player mappings
        CreateMap<PlayerRequestDTO, Player>();
        CreateMap<Player, PlayerResponseDTO>()
            .ForMember(
                dest => dest.TeamName,
                opt => opt.MapFrom(src => src.Team.Name));

        // Referee mappings
        CreateMap<RefereeRequestDTO, Referee>();
        CreateMap<Referee, RefereeResponseDTO>();

        // Tournament mappings
        CreateMap<TournamentRequestDTO, Tournament>();
        CreateMap<Tournament, TournamentResponseDTO>()
            .ForMember(
                dest => dest.TeamsCount,
                opt => opt.MapFrom(src =>
                    src.TournamentTeams != null ? src.TournamentTeams.Count : 0));

        // Sponsor mappings
        CreateMap<SponsorRequestDTO, Sponsor>();
        CreateMap<Sponsor, SponsorResponseDTO>();

        CreateMap<TournamentSponsorRequestDTO, TournamentSponsor>();
        CreateMap<TournamentSponsor, TournamentSponsorResponseDTO>()
            .ForMember(dest => dest.TournamentName, opt => opt.MapFrom(src => src.Tournament.Name))
            .ForMember(dest => dest.SponsorName, opt => opt.MapFrom(src => src.Sponsor.Name));
    }
}
```

---

### 3.4. Controller `SponsorController`

- **Ruta**: `SportsLeague.API/Controllers/SponsorController.cs`
- **Acción**: Crear archivo nuevo:

```csharp
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SponsorController : ControllerBase
{
    private readonly ISponsorService _sponsorService;
    private readonly IMapper _mapper;

    public SponsorController(
        ISponsorService sponsorService,
        IMapper mapper)
    {
        _sponsorService = sponsorService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SponsorResponseDTO>>> GetAll()
    {
        var sponsors = await _sponsorService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<SponsorResponseDTO>>(sponsors));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SponsorResponseDTO>> GetById(int id)
    {
        var sponsor = await _sponsorService.GetByIdAsync(id);
        if (sponsor == null)
            return NotFound(new { message = $"Sponsor con ID {id} no encontrado" });

        return Ok(_mapper.Map<SponsorResponseDTO>(sponsor));
    }

    [HttpPost]
    public async Task<ActionResult<SponsorResponseDTO>> Create(SponsorRequestDTO dto)
    {
        try
        {
            var sponsor = _mapper.Map<Sponsor>(dto);
            var created = await _sponsorService.CreateAsync(sponsor);
            var responseDto = _mapper.Map<SponsorResponseDTO>(created);

            return CreatedAtAction(
                nameof(GetById),
                new { id = responseDto.Id },
                responseDto);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, SponsorRequestDTO dto)
    {
        try
        {
            var sponsor = _mapper.Map<Sponsor>(dto);
            await _sponsorService.UpdateAsync(id, sponsor);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _sponsorService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // POST /api/Sponsor/{id}/tournaments
    [HttpPost("{id}/tournaments")]
    public async Task<ActionResult<TournamentSponsorResponseDTO>> LinkSponsorToTournament(
        int id,
        TournamentSponsorRequestDTO dto)
    {
        try
        {
            var linked = await _sponsorService.LinkSponsorToTournamentAsync(id, dto.TournamentId, dto.ContractAmount);
            var responseDto = _mapper.Map<TournamentSponsorResponseDTO>(linked);

            return CreatedAtAction(
                nameof(GetTournamentsBySponsor),
                new { id = id },
                responseDto);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // GET /api/Sponsor/{id}/tournaments
    [HttpGet("{id}/tournaments")]
    public async Task<ActionResult<IEnumerable<TournamentSponsorResponseDTO>>> GetTournamentsBySponsor(int id)
    {
        try
        {
            var links = await _sponsorService.GetTournamentsBySponsorAsync(id);
            return Ok(_mapper.Map<IEnumerable<TournamentSponsorResponseDTO>>(links));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // DELETE /api/Sponsor/{id}/tournaments/{tournamentId}
    [HttpDelete("{id}/tournaments/{tournamentId}")]
    public async Task<ActionResult> UnlinkSponsorFromTournament(int id, int tournamentId)
    {
        try
        {
            await _sponsorService.UnlinkSponsorFromTournamentAsync(id, tournamentId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
```

---

## 4. Registro en Program.cs

- **Ruta**: `SportsLeague.API/Program.cs`
- **Acción**: Agregar las dependencias de los nuevos repositorios y servicios en la sección de DI.

En la parte de **Repositories**:

```csharp
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IRefereeRepository, RefereeRepository>();          
builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();     
builder.Services.AddScoped<ITournamentTeamRepository, TournamentTeamRepository>(); 
builder.Services.AddScoped<ISponsorRepository, SponsorRepository>();
builder.Services.AddScoped<ITournamentSponsorRepository, TournamentSponsorRepository>();
```

En la parte de **Services**:

```csharp
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IRefereeService, RefereeService>();          
builder.Services.AddScoped<ITournamentService, TournamentService>();     
builder.Services.AddScoped<ISponsorService, SponsorService>();
```

---

## 5. Migración y base de datos

### 5.1. Crear migración

En tu entorno (VS / CLI), asegúrate de:
- **Proyecto de migración**: `SportsLeague.DataAccess`
- **Startup project**: `SportsLeague.API`

Comando:

```powershell
dotnet ef migrations add AddSponsor_TournamentSponsor --project "SportsLeague.DataAccess" --startup-project "SportsLeague.API"
```

Se generan archivos similares a:
- `SportsLeague.DataAccess/Migrations/20260326205429_AddSponsor_TournamentSponsor.cs`
- `SportsLeague.DataAccess/Migrations/20260326205429_AddSponsor_TournamentSponsor.Designer.cs`

La migración crea las tablas `Sponsors` y `TournamentSponsors` con:
- Índice único en `Sponsors.Name`
- Índice compuesto único en `(TournamentId, SponsorId)`

### 5.2. Aplicar migración

```powershell
dotnet ef database update --project "SportsLeague.DataAccess" --startup-project "SportsLeague.API"
```

O desde Package Manager Console (si prefieres):

- `Default project`: `SportsLeague.DataAccess`
- Comando: `Update-Database`

---

## 6. Endpoints a probar en Swagger

1. **POST** `/api/Sponsor`
   - Crear Sponsor (espera 201 o 409).

2. **POST** `/api/Sponsor/{id}/tournaments`
   - Vincular sponsor a torneo (201 o 409).

3. **GET** `/api/Sponsor/{id}/tournaments`
   - Listar torneos del sponsor (200).

4. **PUT** `/api/Sponsor/{id}`
   - Actualizar sponsor (204 o 404).

Con este documento puedes replicar **archivo por archivo** en otro proyecto e incluso copiar el contenido directamente a un `.docx` si lo necesitas.

