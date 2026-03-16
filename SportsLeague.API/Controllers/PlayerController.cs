using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayerController : ControllerBase
{
    private readonly IPlayerService _playerService;
    private readonly IMapper _mapper;

    public PlayerController(
        IPlayerService playerService,
        IMapper mapper)
    {
        _playerService = playerService;
        _mapper = mapper;
    
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlayerResponseDTO>>> GetAll()
    {
        var players = await _playerService.GetAllAsync();
        var playersDto = _mapper.Map<IEnumerable<PlayerResponseDTO>>(players);
        return Ok(playersDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PlayerResponseDTO>> GetById(int id)
    {
        var player = await _playerService.GetByIdAsync(id);

        if (player == null)
            return NotFound(new { message = $"Jugador con ID {id} no encontrado" });

        var playerDto = _mapper.Map<PlayerResponseDTO>(player);
        return Ok(playerDto);
    }

    [HttpGet("team/{teamId}")]
    public async Task<ActionResult<IEnumerable<PlayerResponseDTO>>> GetByTeam(int teamId)
    {
        try
        {
            var players = await _playerService.GetByTeamAsync(teamId);
            var playersDto = _mapper.Map<IEnumerable<PlayerResponseDTO>>(players);
            return Ok(playersDto);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<PlayerResponseDTO>> Create(PlayerRequestDTO dto)
    {
        try
        {
            var player = _mapper.Map<Player>(dto);
            var createdPlayer = await _playerService.CreateAsync(player);

            // Recargar con Team para el response
            var playerWithTeam = await _playerService.GetByIdAsync(createdPlayer.Id);
            var responseDto = _mapper.Map<PlayerResponseDTO>(playerWithTeam);

            return CreatedAtAction(
                nameof(GetById),
                new { id = responseDto.Id },
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

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, PlayerRequestDTO dto)
    {
        try
        {
            var player = _mapper.Map<Player>(dto);
            await _playerService.UpdateAsync(id, player);
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
            await _playerService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
