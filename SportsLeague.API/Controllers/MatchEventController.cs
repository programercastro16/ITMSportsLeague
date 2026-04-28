using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers;
[ApiController]
[Route("api/match/{matchId}")]
public class MatchEventController : ControllerBase

{
    private readonly IMatchEventService _matchEventService;
    private readonly IMapper _mapper;

    public MatchEventController(
        IMatchEventService matchEventService, IMapper mapper)
    {
        _matchEventService = matchEventService;
        _mapper = mapper;
    }

    // ═══ Result ═══ 
    [HttpPost("result")]
    public async Task<ActionResult<MatchResultResponseDTO>> RegisterResult(
        int matchId, MatchResultRequestDTO dto)
    {
        try

        {
            var result = _mapper.Map<MatchResult>(dto);
            var created = await _matchEventService.RegisterResultAsync(matchId, result);
            return Ok(_mapper.Map<MatchResultResponseDTO>(created));
        }

        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpGet("result")]
    public async Task<ActionResult<MatchResultResponseDTO>> GetResult(int matchId)

    {
        try

        {
            var result = await _matchEventService.GetResultByMatchAsync(matchId);
            if (result == null)
                return NotFound(new { message = "Este partido aún no tiene resultado" });
            return Ok(_mapper.Map<MatchResultResponseDTO>(result));
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    // ═══ Goals ═══ 
    [HttpPost("goals")]
    public async Task<ActionResult<GoalResponseDTO>> RegisterGoal(
        int matchId, GoalRequestDTO dto)
    {
        try

        {
            var goal = _mapper.Map<Goal>(dto);
            var created = await _matchEventService.RegisterGoalAsync(matchId, goal);
            var goals = await _matchEventService.GetGoalsByMatchAsync(matchId);
            var createdGoal = goals.FirstOrDefault(g => g.Id == created.Id);
            return Ok(_mapper.Map<GoalResponseDTO>(createdGoal));
        }

        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpGet("goals")]
    public async Task<ActionResult<IEnumerable<GoalResponseDTO>>> GetGoals(int matchId)

    {

        try

        {
            var goals = await _matchEventService.GetGoalsByMatchAsync(matchId);
            return Ok(_mapper.Map<IEnumerable<GoalResponseDTO>>(goals));
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpDelete("goals/{goalId}")]
    public async Task<ActionResult> DeleteGoal(int matchId, int goalId)

    {
        try { await _matchEventService.DeleteGoalAsync(goalId); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    // ═══ Cards ═══ 

    [HttpPost("cards")]
    public async Task<ActionResult<CardResponseDTO>> RegisterCard(
        int matchId, CardRequestDTO dto)

    {
        try

        {
            var card = _mapper.Map<Card>(dto);
            var created = await _matchEventService.RegisterCardAsync(matchId, card);
            var cards = await _matchEventService.GetCardsByMatchAsync(matchId);
            var createdCard = cards.FirstOrDefault(c => c.Id == created.Id);
            return Ok(_mapper.Map<CardResponseDTO>(createdCard));

        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpGet("cards")]
    public async Task<ActionResult<IEnumerable<CardResponseDTO>>> GetCards(int matchId)

    {

        try

        {
            var cards = await _matchEventService.GetCardsByMatchAsync(matchId);
            return Ok(_mapper.Map<IEnumerable<CardResponseDTO>>(cards));

        }

        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }

    }

    [HttpDelete("cards/{cardId}")]
    public async Task<ActionResult> DeleteCard(int matchId, int cardId)

    {
        try { await _matchEventService.DeleteCardAsync(cardId); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

}