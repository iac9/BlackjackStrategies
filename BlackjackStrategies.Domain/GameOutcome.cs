namespace BlackjackStrategies.Domain;

public class GameOutcome
{
    public required GameResult GameResult { get; set; }
    public required Hand PlayerHand { get; set; }
    public required Hand DealerHand { get; set; }
    public decimal Money { get; set; }
    public required bool Doubled { get; set; }
    public required bool Split { get; set; }
    public required int CardsRemaining { get; set; }
}

public enum GameResult
{
    Win,
    Lose,
    Push,
    Blackjack
}