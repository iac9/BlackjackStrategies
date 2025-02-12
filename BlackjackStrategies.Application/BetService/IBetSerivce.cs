using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application.BetService;

public interface IBetSerivce
{
    public decimal Amount { get; set; }
    public decimal SingleBetSize { get; set; }
    public void MakeBet(GameOutcome gameOutcome);
}