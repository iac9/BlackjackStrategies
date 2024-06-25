using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application.BetService
{
    public class MartingaleBetService() : IBetService
    {
        public IEnumerable<decimal> GetAmountOverTime(decimal startingAmount, decimal bettingAmount, IEnumerable<GameOutcome> gameOutcomes)
        {
            var profitLossOverTime = new List<decimal>();
            var currentAmount = startingAmount;
            var consecutiveLosses = 0;

            foreach (GameOutcome outcome in gameOutcomes)
            {
                var bet = Math.Min(currentAmount, bettingAmount * (outcome.Doubled ? 2 : 1) * (decimal)Math.Pow(2, consecutiveLosses));

                switch (outcome.GameResult)
                {
                    case GameResult.Win:
                        currentAmount += bet;
                        consecutiveLosses = 0;
                        break;
                    case GameResult.Blackjack:
                        currentAmount += bet * 1.5M;
                        consecutiveLosses = 0;
                        break;
                    case GameResult.Lose:
                        currentAmount -= bet;
                        consecutiveLosses++;
                        break;
                    default:
                        break;
                }

                profitLossOverTime.Add(currentAmount - startingAmount                  );
            }

            return profitLossOverTime;
        }
    }
}
