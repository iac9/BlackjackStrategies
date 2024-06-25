using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application.BetService
{
    public class HiLoBetService : IBetService
    {
        private int runningCount = 0;

        public IEnumerable<decimal> GetAmountOverTime(decimal startingAmount, decimal bettingAmount, IEnumerable<GameOutcome> gameOutcomes)
        {
            var gameOutcomeArray = gameOutcomes.ToArray();
            var profitLossOverTime = new List<decimal>();
            var profitLoss = startingAmount;

            for (int i = 0; i < gameOutcomeArray.Length; i++)
            {
                var gameOutcome = gameOutcomeArray[i];
                UpdateRunningCount(gameOutcome.PlayerHand);
                UpdateRunningCount(gameOutcome.DealerHand);

                var trueCount = i == 0 ? GetTrueCount() : GetTrueCount(gameOutcomeArray[i - 1].CardsRemaining);
                var bet = Math.Min(profitLoss, bettingAmount * (gameOutcome.Doubled ? 2 : 1) * trueCount);

                profitLoss += gameOutcome.GameResult switch
                {
                    GameResult.Win => bet,
                    GameResult.Lose => -bet,
                    GameResult.Blackjack => bet * 1.5M,
                    _ => 0,
                };

                profitLossOverTime.Add(profitLoss - startingAmount);
            }

            return profitLossOverTime;
        }

        private int GetTrueCount(int? cardsRemaining = null)
        {
            if (cardsRemaining == null)
                return runningCount;
            else
                return Math.Max((int)Math.Round(runningCount / (decimal)cardsRemaining / 52), 1);

        }

        private void UpdateRunningCount(Hand hand)
        {
            foreach (var card in hand.Cards)
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