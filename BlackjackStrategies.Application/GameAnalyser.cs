using BlackjackStrategies.Application.BetService;
using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application
{
    public interface IGameAnalyser
    {
        decimal GetExpectedValue(IEnumerable<GameOutcome> gameOutcomes);
        Dictionary<GameResult, int> GetGameResultCount(IEnumerable<GameOutcome> gameOutcomes);
        decimal GetProfitLoss(decimal startingAmount, decimal bettingAmount, IEnumerable<GameOutcome> gameOutcomes);
    }

    public class GameAnalyser(IBetService betService) : IGameAnalyser
    {
        public decimal GetExpectedValue(IEnumerable<GameOutcome> gameOutcomes)
        {
            var gameResultCount = GetGameResultCount(gameOutcomes);

            var expectedValue = 0M;

            foreach (var gameResult in gameResultCount.Keys)
            {
                var count = gameResultCount[gameResult];
                var probability = count / (decimal)gameOutcomes.Count();

                expectedValue += gameResult switch
                {
                    GameResult.Win => probability,
                    GameResult.Lose => -probability,
                    GameResult.Blackjack => 1.5M * probability,
                    _ => 0,
                };
            }

            return expectedValue;
        }

        public Dictionary<GameResult, int> GetGameResultCount(IEnumerable<GameOutcome> gameOutcomes) =>
            gameOutcomes.GroupBy(o => o.GameResult).ToDictionary(g => g.Key, g => g.Count());

        public decimal GetProfitLoss(decimal startingAmount, decimal bettingAmount, IEnumerable<GameOutcome> gameOutcomes)
        {
            throw new NotImplementedException();
        }
    }
}
