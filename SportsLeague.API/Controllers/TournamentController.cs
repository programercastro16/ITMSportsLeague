using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TournamentController : ControllerBase
{
    private readonly ITournamentService _tournamentService;
    private readonly IMapper _mapper;

    public TournamentController(
        ITournamentService tournamentService,
        IMapper mapper)
    {
        _tournamentService = tournamentService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TournamentResponseDTO>>> GetAll()
    {
        var tournaments = await _tournamentService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<TournamentResponseDTO>>(tournaments));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TournamentResponseDTO>> GetById(int id)
    {
        var tournament = await _tournamentService.GetByIdAsync(id);
        if (tournament == null)
            return NotFound(new { message = $"Torneo con ID {id} no encontrado" });
        return Ok(_mapper.Map<TournamentResponseDTO>(tournament));
    }

    [HttpPost]
    public async Task<ActionResult<TournamentResponseDTO>> Create(TournamentRequestDTO dto)
    {
        try
        {
            var tournament = _mapper.Map<Tournament>(dto);
            var created = await _tournamentService.CreateAsync(tournament);
            var responseDto = _mapper.Map<TournamentResponseDTO>(created);
            return CreatedAtAction(nameof(GetById), new { id = responseDto.Id }, responseDto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, TournamentRequestDTO dto)
    {
        try
        {
            var tournament = _mapper.Map<Tournament>(dto);
            await _tournamentService.UpdateAsync(id, tournament);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _tournamentService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpPatch("{id}/status")]
    public async Task<ActionResult> UpdateStatus(int id, UpdateStatusDTO dto)
    {
        try
        {
            await _tournamentService.UpdateStatusAsync(id, dto.Status);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpPost("{id}/teams")]
    public async Task<ActionResult> RegisterTeam(int id, RegisterTeamDTO dto)
    {
        try
        {
            await _tournamentService.RegisterTeamAsync(id, dto.TeamId);
            return Ok(new { message = "Equipo inscrito exitosamente" });
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpGet("{id}/teams")]
    public async Task<ActionResult<IEnumerable<TeamResponseDTO>>> GetTeams(int id)
    {
        try
        {
            var teams = await _tournamentService.GetTeamsByTournamentAsync(id);
            return Ok(_mapper.Map<IEnumerable<TeamResponseDTO>>(teams));
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }
}
