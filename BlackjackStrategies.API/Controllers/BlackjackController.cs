using BlackjackStrategies.Application;
using Microsoft.AspNetCore.Mvc;

namespace BlackjackStrategies.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlackjackController(IGameSimulator simulator, IGameAnalyser analyser) : ControllerBase
    {
        [HttpPost("simulate")]
        public ActionResult<GameStatistic> SimulateGame([FromBody] SimulateGameRequest request)
        {
            var gameOutcomes = simulator.Simulate(
                request.NumberOfDecks, 
                request.NumberOfGames, 
                request.StartingAmount, 
                request.BettingSize, 
                request.StrategyType
            );

            var gameStatistic = new GameStatistic
            {
                GameOutcomes = gameOutcomes,
                ExpectedValue = analyser.GetExpectedValue(gameOutcomes),
                GameResultCount = analyser.GetGameResultCount(gameOutcomes)
            };

            return Ok(gameStatistic);
        }
    }
}
