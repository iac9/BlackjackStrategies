using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application.BetService
{
    public class MartingaleBetService() : IBetService
    {
        public IEnumerable<decimal> GetAmountOverTime(decimal startingAmount, decimal bettingAmount, IEnumerable<GameOutcome> gameOutcomes)
        {
            var profitLossOverTime = new List<decimal>();
            var profitLoss = startingAmount;
            var consecutiveLosses = 0;

            foreach (GameOutcome outcome in gameOutcomes)
            {
                var bet = Math.Min(profitLoss, bettingAmount * (outcome.Doubled ? 2 : 1) * (decimal)Math.Pow(2, consecutiveLosses));

                switch (outcome.GameResult)
                {
                    case GameResult.Win:
                        profitLoss += bet;
                        consecutiveLosses = 0;
                        break;
                    case GameResult.Blackjack:
                        profitLoss += bet * 1.5M;
                        consecutiveLosses = 0;
                        break;
                    case GameResult.Lose:
                        profitLoss -= bet;
                        consecutiveLosses++;
                        break;
                    default:
                        break;
                }

                profitLossOverTime.Add(profitLoss - startingAmount                  );
            }

            return profitLossOverTime;
        }
    }
}
