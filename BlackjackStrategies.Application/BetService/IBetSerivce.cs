using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application.BetService
{
    public interface IBetSerivce
    {
        public void MakeBet(GameOutcome gameOutcome);
    }
}
