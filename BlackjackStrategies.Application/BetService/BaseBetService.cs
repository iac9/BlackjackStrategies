using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application.BetService;

public abstract class BaseBetService : IBetService
{
    public decimal Amount { get; set; }
    public decimal SingleBetSize { get; set; }

    public abstract void MakeBet(GameOutcome gameOutcome);

    protected void UpdateAmount(GameOutcome gameOutcome, decimal betAmount) 
    {
        Amount += gameOutcome.GameResult switch
        {
            GameResult.Win => betAmount,
            GameResult.Lose => -betAmount,
            GameResult.Blackjack => betAmount * 1.5M,
            _ => 0,
        };

        gameOutcome.Money = Amount;
    }
}