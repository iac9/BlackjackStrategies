using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application.ActionService;

public interface IDealer
{
    public Hand Hand { get; set; }
    public HandAction GetAction();
    public void ResetState();
}

public class Dealer : IDealer
{
    public Hand Hand { get; set; } = new();
    public HandAction GetAction()
    {
        return Hand.GetValue() < Constants.DealerStayThreshold ? HandAction.Hit : HandAction.Stay;
    }

    public void ResetState()
    {
        Hand.Clear();
    }
}