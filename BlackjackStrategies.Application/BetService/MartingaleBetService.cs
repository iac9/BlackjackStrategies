using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application.BetService
{
    public class MartingaleBetService(decimal startingAmount, decimal bettingSize) : IBetSerivce
    {
        private readonly decimal startingAmount = startingAmount;
        private int consecutiveLosses = 0;
        private decimal currentAmount = startingAmount;

        public void MakeBet(GameOutcome gameOutcome)
        {
            var doubledFactor = gameOutcome.Doubled ? 2 : 1;
            var consecutiveLossesFactor = (decimal)Math.Pow(2, consecutiveLosses);
            var bettingAmount = Math.Min(currentAmount, bettingSize * consecutiveLossesFactor * doubledFactor);

            switch (gameOutcome.GameResult)
            {
                case GameResult.Win:
                    currentAmount += bettingAmount;
                    consecutiveLosses = 0;
                    break;
                case GameResult.Blackjack:
                    currentAmount += bettingAmount * 1.5M;
                    consecutiveLosses = 0;
                    break;
                case GameResult.Lose:
                    currentAmount -= bettingAmount;
                    consecutiveLosses++;
                    break;
                default:
                    break;
            }

            gameOutcome.Money = currentAmount - startingAmount;
        }
    }
}
