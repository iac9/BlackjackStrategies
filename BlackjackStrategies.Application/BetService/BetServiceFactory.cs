using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application.BetService;

public interface IBetServiceFactory
{
    IBetSerivce GetBetService(StrategyType strategyType, decimal startingAmount, decimal bettingSize);
}

public class BetServiceFactory : IBetServiceFactory
{
    public IBetSerivce GetBetService(StrategyType strategyType, decimal startingAmount, decimal bettingSize)
    {
        return strategyType switch
        {
            StrategyType.Martingale => new MartingaleBetService { Amount = startingAmount, SingleBetSize = bettingSize },
            StrategyType.HiLo => new HiLoBetService { Amount = startingAmount, SingleBetSize = bettingSize },
            _ => throw new KeyNotFoundException($"Bet service with strategy '{strategyType}' not found."),
        };
    }
}