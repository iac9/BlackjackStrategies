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

            Console.WriteLine(profitLoss);
            foreach (GameOutcome outcome in gameOutcomes)
            {
                var betSize = Math.Min(profitLoss, bettingAmount * (outcome.Doubled ? 2 : 1) * (decimal)Math.Pow(2, consecutiveLosses));

                switch (outcome.GameResult)
                {
                    case GameResult.Win:
                        profitLoss += betSize;
                        consecutiveLosses = 0;
                        break;
                    case GameResult.Blackjack:
                        profitLoss += betSize * 1.5M;
                        consecutiveLosses = 0;
                        break;
                    case GameResult.Lose:
                        profitLoss -= betSize;
                        consecutiveLosses++;
                        break;
                    default:
                        break;
                }

                profitLossOverTime.Add(profitLoss);
            }

            return profitLossOverTime;
        }
    }
}
