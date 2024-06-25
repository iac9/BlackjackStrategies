using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application.BetService
{
    public interface IBetService
    {
        public decimal GetAmountToBet(decimal startingAmount, IEnumerable<GameOutcome> previousGameOutcomes);
    }
}
