using BlackjackStrategies.Domain;

namespace BlackjackStrategies.API
{
    public class GameStatistic
    {
        public required IEnumerable<GameOutcome> GameOutcomes { get; set; }
        public decimal ExpectedValue { get; set; }
        public required Dictionary<GameResult, int> GameResultCount { get; set; }
    }
}
