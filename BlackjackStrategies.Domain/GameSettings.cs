namespace BlackjackStrategies.Domain;

public enum StrategyType
{
    Martingale,
    HiLo
}

public class GameSettings
{
    public int NumberOfDecks { get; init; }
    public int NumberOfGames { get; init; }
    public decimal StartingAmount { get; init; }
    public decimal BettingSize { get; init; }
    public StrategyType StrategyType { get; init; }
    public bool AutomaticShuffler { get; init; }
}