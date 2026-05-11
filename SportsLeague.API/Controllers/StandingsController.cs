using Microsoft.AspNetCore.Mvc;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers;

[ApiController]
[Route("api")]

public class StandingsController : ControllerBase

{
    private readonly IStandingsService _standingsService;
    public StandingsController(IStandingsService standingsService)

    {
        _standingsService = standingsService;
    }

    [HttpGet("standings")]
    public async Task<ActionResult> GetStandings([FromQuery] int tournamentId)

    {
        try

        {
            var standings = await _standingsService.GetStandingsAsync(tournamentId);
            return Ok(standings);
        }

        catch (KeyNotFoundException ex)

        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("stats/scorers")]
    public async Task<ActionResult> GetTopScorers([FromQuery] int tournamentId)
    {
        try

        {
            var scorers = await _standingsService.GetTopScorersAsync(tournamentId);
            return Ok(scorers);
        }

        catch (KeyNotFoundException ex)

        {
            return NotFound(new { message = ex.Message });
        }

    }

    [HttpGet("stats/cards")]

    public async Task<ActionResult> GetCardStats([FromQuery] int tournamentId)

    {
        try

        {
            var cards = await _standingsService.GetCardStatsAsync(tournamentId);
            return Ok(cards);
        }

        catch (KeyNotFoundException ex)

        {
            return NotFound(new { message = ex.Message });
        }
    }
}