using BlackjackStrategies.API.Models;
using BlackjackStrategies.Application;
using BlackjackStrategies.Domain;
using Microsoft.AspNetCore.Mvc;

namespace BlackjackStrategies.API.Controllers;

[ApiController]
[Route("[controller]")]
public class BlackjackController(IGameSimulator simulator, IGameAnalyser analyser) : ControllerBase
{
    [HttpPost("simulate")]
    public ActionResult<GameReportResponse> SimulateGame([FromBody] GameSettings settings)
    {
        try
        {
            var gameOutcomes = simulator.Simulate(settings).ToArray();
            var gameStatistics = analyser.GetGameStatistics(gameOutcomes);

            var gameReport = new GameReportResponse
            {
                MoneyOverTime = gameOutcomes.Select(o => o.Money).Where(m => m > 0),
                NumberOfGamesSimulated = gameOutcomes.Length,
                NumberOfGamesPlayed = gameStatistics.NumberOfGamesPlayed,
                ExpectedValue = gameStatistics.ExpectedValue,
                GameResultCount = gameStatistics.GameResultCount
            };

            return Ok(gameReport);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }
}