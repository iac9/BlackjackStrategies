using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application.BetService;

public class MartingaleBetService : BaseBetService
{
    private int _consecutiveLosses = 0;

    public override void MakeBet(GameOutcome gameOutcome)
    {
        if (Amount == 0)
            return;

        UpdateAmount(gameOutcome, GetAmountToBet(gameOutcome.Doubled));

        _consecutiveLosses = gameOutcome.GameResult switch
        {
            GameResult.Win or GameResult.Blackjack => 0,
            GameResult.Lose => _consecutiveLosses + 1,
            _ => _consecutiveLosses,
        };
    }

    private decimal GetAmountToBet(bool doubled)
    {
        var doubledFactor = doubled ? 2 : 1;
        var desiredAmountToBet = SingleBetSize * doubledFactor * (decimal)Math.Pow(2, _consecutiveLosses);

        return Math.Min(Amount, desiredAmountToBet);
    }
}