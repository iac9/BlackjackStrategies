using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application.BetService
{
    public class ConstantBetService : IBetService
    {
        public IEnumerable<decimal> GetAmountOverTime(decimal startingAmount, decimal bettingAmount, IEnumerable<GameOutcome> gameOutcomes)
        {
            var currentAmount = startingAmount;
            var profitLossOverTime = new List<decimal>();

            foreach (var outcome in gameOutcomes)
            {
                var bet = Math.Min(currentAmount, bettingAmount * (outcome.Doubled ? 2 : 1));

                currentAmount += outcome.GameResult switch
                {
                    GameResult.Win => bettingAmount,
                    GameResult.Lose => -bettingAmount,
                    GameResult.Blackjack => 1.5M * bettingAmount,
                    _ => 0,
                };

                profitLossOverTime.Add(currentAmount - startingAmount);
            }

            return profitLossOverTime;
        }
    }
}
