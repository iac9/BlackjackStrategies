using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application
{
    public interface IGameAnalyser
    {
        public GameStatistic GetGameStatistics(IEnumerable<GameOutcome> gameOutcomes);
    }

    public class GameStatistic
    {
        public required int NumberOfGamesPlayed { get; set; }
        public required decimal ExpectedValue { get; set; }
        public required Dictionary<GameResult, int> GameResultCount { get; set; }
    }

    public class GameAnalyser : IGameAnalyser
    {
        public GameStatistic GetGameStatistics(IEnumerable<GameOutcome> gameOutcomes)
        {
            return new GameStatistic
            {
                NumberOfGamesPlayed = gameOutcomes.Where(o => o.Money > 0).Count() + 1,
                ExpectedValue = GetExpectedValue(gameOutcomes),
                GameResultCount = GetGameResultCount(gameOutcomes)
            };
        }

        private static decimal GetExpectedValue(IEnumerable<GameOutcome> gameOutcomes)
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

        private static Dictionary<GameResult, int> GetGameResultCount(IEnumerable<GameOutcome> gameOutcomes) =>
            gameOutcomes.GroupBy(o => o.GameResult).ToDictionary(g => g.Key, g => g.Count());
    }
}
