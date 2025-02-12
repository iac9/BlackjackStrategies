using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application.ActionService;

public interface IPlayerService
{
    public Hand Hand { get; set; }
    public IEnumerable<Hand>? SplitHands { get; set; }
    public bool Doubled { get; set; }
    public void ResetState();
    HandAction GetAction(Card dealerUpCard);
}

public enum HandAction
{
    Hit,
    Stay,
    Split,
    Double
}