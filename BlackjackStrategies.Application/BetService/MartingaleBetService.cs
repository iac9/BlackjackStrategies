using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application.BetService
{
    public class MartingaleBetService : BaseBetService
    {
        private int consecutiveLosses = 0;

        public override void MakeBet(GameOutcome gameOutcome)
        {
            UpdateAmount(gameOutcome, GetAmountToBet(gameOutcome.Doubled));

            consecutiveLosses = gameOutcome.GameResult switch
            {
                GameResult.Win or GameResult.Blackjack => 0,
                GameResult.Lose => consecutiveLosses + 1,
                _ => consecutiveLosses,
            };
        }

        private decimal GetAmountToBet(bool doubled)
        {
            var doubledFactor = doubled ? 2 : 1;
            var desiredAmountToBet = SingleBetSize * doubledFactor * (decimal)Math.Pow(2, consecutiveLosses);

            return Math.Min(Amount, desiredAmountToBet);
        }
    }
}
