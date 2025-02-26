namespace BlackjackStrategies.Domain;

public enum StrategyType
{
    Martingale,
    HiLo
}

public class GameSettings
{
    private readonly int _numberOfDecks;
    private readonly decimal _startingAmount;
    private readonly decimal _bettingSize;

    public int NumberOfDecks
    {
        get => _numberOfDecks;
        init => _numberOfDecks =
            value > 0 ? value : throw new ArgumentException("Number of games must be greater than 0.");
    }
    
    public decimal StartingAmount
    {
        get => _startingAmount;
        init => _startingAmount = 
            value > 0 ? value : throw new ArgumentException("Starting amount must be greater than 0.");
    }

    public decimal BettingSize
    {
        get => _bettingSize;
        init
        {
            if (value < 0)
                throw new ArgumentException("Betting size must be greater than 0.");
            
            if (_bettingSize > _startingAmount) 
                throw new ArgumentException("Betting cannot exceed starting amount.");
            
            _bettingSize = value;
        }
    }

    public StrategyType StrategyType { get; init; }
    public bool AutomaticShuffler { get; init; }
}