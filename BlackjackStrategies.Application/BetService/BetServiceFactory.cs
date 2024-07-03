namespace BlackjackStrategies.Application.BetService
{
    public interface IBetServiceFactory
    {
        IBetSerivce GetBetSerivce(StrategyType strategyType, decimal startingAmount, decimal bettingSize);
    }

    public enum StrategyType
    {
        Martingale,
        HiLo
    }

    public class BetServiceFactory : IBetServiceFactory
    {
        public IBetSerivce GetBetSerivce(StrategyType strategyType, decimal startingAmount, decimal bettingSize)
        {
            return strategyType switch
            {
                StrategyType.Martingale => new MartingaleBetService { Amount = startingAmount, SingleBetSize = bettingSize },
                StrategyType.HiLo => new HiLoBetService { Amount = startingAmount, SingleBetSize = bettingSize },
                _ => throw new KeyNotFoundException($"Bet service with strategy '{strategyType}' not found."),
            };
        }
    }
}
