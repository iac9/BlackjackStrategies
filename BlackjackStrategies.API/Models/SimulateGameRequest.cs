using BlackjackStrategies.Application.BetService;

namespace BlackjackStrategies.API.Models
{
    public class SimulateGameRequest
    {
        public int NumberOfGames { get; set; }
        public int NumberOfDecks { get; set; }
        public decimal StartingAmount { get; set; }
        public decimal BettingSize { get; set; }
        public StrategyType StrategyType { get; set; }
    }
}
