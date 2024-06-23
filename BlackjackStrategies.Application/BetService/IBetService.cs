namespace BlackjackStrategies.Application.BetService
{
    public interface IBetService
    {
        public decimal GetProfitLoss(decimal startingAmount, IEnumerable<GameOutcome> gameOutcomes);
    }
}
