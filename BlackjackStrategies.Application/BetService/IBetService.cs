using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application.BetService
{
    public interface IBetService
    {
        public IEnumerable<decimal> GetAmountOverTime(decimal startingAmount, decimal bettingAmount, IEnumerable<GameOutcome> gameOutcomes);
    }
}
