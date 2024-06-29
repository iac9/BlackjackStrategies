using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application.BetService
{
    public class HiLoBetService(decimal startingAmount, decimal bettingSize) : IBetSerivce
    {
        private int runningCount = 0;
        private GameOutcome? lastGameOutcome = null;
        private decimal currentAmount = startingAmount;
        private readonly decimal startingAmount = startingAmount;

        public void MakeBet(GameOutcome gameOutcome)
        {
            UpdateRunningCount(gameOutcome);

            var trueCount = GetTrueCount();
            var bet = Math.Min(currentAmount, bettingSize * (gameOutcome.Doubled ? 2 : 1) * trueCount);

            currentAmount += gameOutcome.GameResult switch
            {
                GameResult.Win => bet,
                GameResult.Lose => -bet,
                GameResult.Blackjack => bet * 1.5M,
                _ => 0,
            };

            gameOutcome.Money = currentAmount - startingAmount;
            lastGameOutcome = gameOutcome;
        }

        private int GetTrueCount()  
        {
            var trueCount = lastGameOutcome == null ? 
                runningCount : 
                (int)Math.Round(runningCount / (decimal)lastGameOutcome.CardsRemaining / 52);

            return Math.Max(trueCount, 1);
        }

        private void UpdateRunningCount(GameOutcome gameOutcome)
        {
            if (lastGameOutcome?.CardsRemaining < gameOutcome.CardsRemaining)
            {
                runningCount = 0;
            }

            var cards = gameOutcome.PlayerHand.Cards.Concat(gameOutcome.DealerHand.Cards);

            foreach (var card in cards)
            {
                runningCount += card.Value switch
                {
                    CardValue.Two or CardValue.Three or CardValue.Four or CardValue.Five or CardValue.Six => 1,
                    CardValue.Ten or CardValue.Jack or CardValue.Queen or CardValue.King or CardValue.Ace => -1,
                    _ => 0,
                };
            }
        }
    }
}