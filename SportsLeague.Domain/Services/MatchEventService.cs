using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Helpers;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services;

public class MatchEventService : IMatchEventService

{
    private readonly IMatchRepository _matchRepository;
    private readonly IMatchResultRepository _matchResultRepository;
    private readonly IGoalRepository _goalRepository;
    private readonly ICardRepository _cardRepository;
    private readonly MatchValidationHelper _validationHelper;
    private readonly ILogger<MatchEventService> _logger;

    public MatchEventService(

        IMatchRepository matchRepository,
        IMatchResultRepository matchResultRepository,
        IGoalRepository goalRepository,
        ICardRepository cardRepository,
        MatchValidationHelper validationHelper,
        ILogger<MatchEventService> logger)

    {

        _matchRepository = matchRepository;
        _matchResultRepository = matchResultRepository;
        _goalRepository = goalRepository;
        _cardRepository = cardRepository;
        _validationHelper = validationHelper;
        _logger = logger;
    }



    // ═══ MatchResult ═══ 

    public async Task<MatchResult> RegisterResultAsync(
        int matchId, MatchResult result)

    {

        var match = await _matchRepository.GetByIdAsync(matchId);
        if (match == null)
            throw new KeyNotFoundException(
                $"No se encontró el partido con ID {matchId}");


        if (match.Status != MatchStatus.Finished)
            throw new InvalidOperationException(
                "Solo se puede registrar resultado en partidos con estado Finished");


        var existingResult = await _matchResultRepository
            .GetByMatchIdAsync(matchId);
        if (existingResult != null)
            throw new InvalidOperationException(
                "Este partido ya tiene un resultado registrado");


        if (result.HomeGoals < 0 || result.AwayGoals < 0)
            throw new InvalidOperationException(
                "Los goles no pueden ser negativos");

        result.MatchId = matchId;

        _logger.LogInformation(
            "Registering result for match {MatchId}: {Home}-{Away}",
            matchId, result.HomeGoals, result.AwayGoals);
        return await _matchResultRepository.CreateAsync(result);
    }

    public async Task<MatchResult?> GetResultByMatchAsync(int matchId)

    {
        var match = await _matchRepository.GetByIdAsync(matchId);
        if (match == null)
            throw new KeyNotFoundException(
                $"No se encontró el partido con ID {matchId}");
        return await _matchResultRepository.GetByMatchIdAsync(matchId);
    }

    // ═══ Goals ═══ 

    public async Task<Goal> RegisterGoalAsync(int matchId, Goal goal)

    {
        var match = await _validationHelper.ValidateMatchForEventAsync(matchId);
        await _validationHelper.ValidatePlayerInMatchAsync(goal.PlayerId, match);
        MatchValidationHelper.ValidateMinute(goal.Minute);
        goal.MatchId = matchId;

        _logger.LogInformation(
            "Registering goal: Match {MatchId}, Player {PlayerId}, Minute {Minute}",
            matchId, goal.PlayerId, goal.Minute);
        return await _goalRepository.CreateAsync(goal);
    }

    public async Task<IEnumerable<Goal>> GetGoalsByMatchAsync(int matchId)

    {
        var match = await _matchRepository.GetByIdAsync(matchId);
        if (match == null)
            throw new KeyNotFoundException(
                $"No se encontró el partido con ID {matchId}");
        return await _goalRepository.GetByMatchWithDetailsAsync(matchId);
    }



    public async Task DeleteGoalAsync(int goalId)

    {
        var exists = await _goalRepository.ExistsAsync(goalId);
        if (!exists)
            throw new KeyNotFoundException(
                $"No se encontró el gol con ID {goalId}");
        await _goalRepository.DeleteAsync(goalId);

    }

    // ═══ Cards ═══ 

    public async Task<Card> RegisterCardAsync(int matchId, Card card)

    {
        var match = await _validationHelper.ValidateMatchForEventAsync(matchId);
        await _validationHelper.ValidatePlayerInMatchAsync(card.PlayerId, match);
        MatchValidationHelper.ValidateMinute(card.Minute);
        card.MatchId = matchId;
        _logger.LogInformation(
            "Registering {CardType} card: Match {MatchId}, Player {PlayerId}",
            card.Type, matchId, card.PlayerId);
        return await _cardRepository.CreateAsync(card);

    }

    public async Task<IEnumerable<Card>> GetCardsByMatchAsync(int matchId)

    {
        var match = await _matchRepository.GetByIdAsync(matchId);
        if (match == null)
            throw new KeyNotFoundException(
                $"No se encontró el partido con ID {matchId}");
        return await _cardRepository.GetByMatchWithDetailsAsync(matchId);
    }

    public async Task DeleteCardAsync(int cardId)

    {
        var exists = await _cardRepository.ExistsAsync(cardId);
        if (!exists)
            throw new KeyNotFoundException(
                $"No se encontró la tarjeta con ID {cardId}");
        await _cardRepository.DeleteAsync(cardId);
    }

}