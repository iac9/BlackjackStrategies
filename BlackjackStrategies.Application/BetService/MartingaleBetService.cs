using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application.BetService
{
    public class MartingaleBetService(decimal portionToWager) : IBetService
    {
        public decimal GetAmountToBet(decimal startingAmount, IEnumerable<GameOutcome> gameOutcomes)
        {
            var profitLoss = startingAmount;
            var consecutiveLosses = 0;

            Console.WriteLine(profitLoss);
            foreach (GameOutcome outcome in gameOutcomes)
            {
                var betSize = Math.Min(profitLoss, startingAmount * (outcome.Doubled ? 2 : 1) * portionToWager * (decimal)Math.Pow(2, consecutiveLosses));

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

                Console.WriteLine(profitLoss);
            }

            return Math.Round(profitLoss, 2);
        }
    }
}
