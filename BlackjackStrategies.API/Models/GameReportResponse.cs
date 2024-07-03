using BlackjackStrategies.Domain;

namespace BlackjackStrategies.API.Models
{
    public class GameReportResponse
    {
        public required IEnumerable<decimal> MoneyOverTime { get; set; }
        public required int NumberOfGamesSimulated { get; set; }
        public required int NumberOfGamesPlayed { get; set; }
        public decimal ExpectedValue { get; set; }
        public required Dictionary<GameResult, int> GameResultCount { get; set; }
    }
}
