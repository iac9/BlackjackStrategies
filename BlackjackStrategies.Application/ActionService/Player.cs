using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application.ActionService;

public abstract class BasePlayer
{
    private int _currenHandIndex;
    public List<Hand> Hands { get; set; } = [new()];
    public Hand CurrentHand => Hands[_currenHandIndex];
    public bool Doubled { get; set; }

    public void ResetState()
    {
        _currenHandIndex = 0;
        Hands = [new Hand()];
        Doubled = false;
    }

    public bool NextHand()
    { 
        _currenHandIndex = Math.Min(_currenHandIndex + 1, Hands.Count - 1);
        
        return _currenHandIndex < Hands.Count;
    }
    
    public abstract HandAction GetAction(Card dealerUpCard);
}

public enum HandAction
{
    Hit,
    Stay,
    Double,
    Split
}