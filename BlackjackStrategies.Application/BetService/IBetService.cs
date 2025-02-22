using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application.BetService;

public interface IBetService
{
    public decimal Amount { get; set; }
    public decimal SingleBetSize { get; set; }
    public void MakeBet(GameOutcome gameOutcome);
}