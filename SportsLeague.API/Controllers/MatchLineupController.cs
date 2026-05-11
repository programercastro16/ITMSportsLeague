using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;
namespace SportsLeague.API.Controllers;

[ApiController]
[Route("api/match/{matchId}/lineup")]
public class MatchLineupController : ControllerBase
{
    private readonly IMatchLineupService _matchLineupService;
    private readonly IMapper _mapper;
    public MatchLineupController(
    IMatchLineupService matchLineupService,
    
IMapper mapper)
    {
        _matchLineupService = matchLineupService;
        _mapper = mapper;
    }
    [HttpPost]
    public async Task<ActionResult<MatchLineupDto>> Create(
    int matchId,
    CreateMatchLineupDto dto)
    {
        try
        {
            var lineup = _mapper.Map<MatchLineup>(dto);
            lineup.MatchId = matchId;
            var created = await _matchLineupService.CreateAsync(lineup);
            return Created(
            string.Empty,
            _mapper.Map<MatchLineupDto>(created));
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
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MatchLineupDto>>> GetAll(
    int matchId)
    {
        var lineups = await _matchLineupService.GetByMatchAsync(matchId);
        return Ok(_mapper.Map<IEnumerable<MatchLineupDto>>(lineups));
    }
    [HttpGet("team/{teamId}")]
    public async Task<ActionResult<IEnumerable<MatchLineupDto>>> GetByTeam(
    int matchId,
    int teamId)
    {
        var lineups = await _matchLineupService
        .GetByMatchAndTeamAsync(matchId, teamId);
        
    return Ok(_mapper.Map<IEnumerable<MatchLineupDto>>(lineups));
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int matchId, int id)
    {
        try
        {
            await _matchLineupService.DeleteAsync(matchId, id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}