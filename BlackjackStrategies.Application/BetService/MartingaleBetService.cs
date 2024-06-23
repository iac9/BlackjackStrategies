namespace BlackjackStrategies.Application.BetService
{
    public class MartingaleBetService(decimal portionToWager) : IBetService
    {
        public decimal GetProfitLoss(decimal startingAmount, IEnumerable<GameOutcome> gameOutcomes)
        {
            var profitLoss = startingAmount;
            var consecutiveLosses = 0;

            foreach (GameOutcome outcome in gameOutcomes)
            {
                var betSize = Math.Min(profitLoss, startingAmount * portionToWager * (decimal)Math.Pow(2, consecutiveLosses));

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
            }

            return Math.Round(profitLoss, 2);
        }
    }
}
