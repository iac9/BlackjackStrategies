using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application.ActionService;

public class BasicStrategyPlayerService : IPlayerService
{
    public Hand Hand { get; set; } = new();
    public IEnumerable<Hand>? SplitHands { get; set; }
    public bool Doubled { get; set; }
    public void ResetState()
    {
        Hand.Clear();
        SplitHands = null;
        Doubled = false;
    }

    public HandAction GetAction(Card dealerUpCard)
    {
        if (Hand.Has2Cards)
        {
            if (Hand.Cards.First().Value == Hand.Cards.Last().Value &&
                ShouldSplit(dealerUpCard))
            {
                return HandAction.Split;
            }
            else if (Hand.Cards.Any(c => c.Value == CardValue.Ace))
            {
                return HandHasAce(dealerUpCard);
            }
        }
        return HandHasNoAce(dealerUpCard);
    }

    private HandAction HandHasNoAce(Card dealerUpCard)
    {
        var handValue = Hand.GetValue();

        if (Hand.Has2Cards)
        {
            if (handValue == 9)
                return CardValueExtensions.InRange(CardValue.Three, CardValue.Six, dealerUpCard.Value) ?
                    HandAction.Double : HandAction.Hit;

            if (handValue == 10)
                return CardValueExtensions.InRange(CardValue.Two, CardValue.Nine, dealerUpCard.Value) ?
                    HandAction.Double : HandAction.Hit;

            if (handValue == 11)
                return HandAction.Double;
        }

        if (handValue < 9)
            return HandAction.Hit;

        if (handValue == 12)
            return CardValueExtensions.InRange(CardValue.Four, CardValue.Six, dealerUpCard.Value) ?
                HandAction.Stay : HandAction.Hit;

        if (13 <= handValue && handValue <= 16)
            return CardValueExtensions.InRange(CardValue.Two, CardValue.Six, dealerUpCard.Value) ?
                HandAction.Stay : HandAction.Hit;

        return HandAction.Stay;
    }



    private HandAction HandHasAce(Card dealerUpCard)
    {
        var playerOtherCard = Hand.Cards.FirstOrDefault(c => c.Value != CardValue.Ace) ?? Hand.Cards.First();

        return playerOtherCard.Value switch
        {
            CardValue.Two or CardValue.Three => CardValueExtensions.InRange(CardValue.Five, CardValue.Six, dealerUpCard.Value) ? 
                HandAction.Double : HandAction.Hit,
            CardValue.Four or CardValue.Five => CardValueExtensions.InRange(CardValue.Four, CardValue.Six, dealerUpCard.Value) ? 
                HandAction.Double : HandAction.Hit,
            CardValue.Six => CardValueExtensions.InRange(CardValue.Three, CardValue.Six, dealerUpCard.Value) ? 
                HandAction.Double : HandAction.Hit,
            CardValue.Seven => CardValueExtensions.InRange(CardValue.Two, CardValue.Six, dealerUpCard.Value) ?
                HandAction.Double : CardValueExtensions.InRange(CardValue.Seven, CardValue.Eight, dealerUpCard.Value) ?
                    HandAction.Stay : HandAction.Hit,
            CardValue.Eight => dealerUpCard.Value == CardValue.Six ? 
                HandAction.Double : HandAction.Stay,
            _ => HandAction.Stay,
        };
    }

    private bool ShouldSplit(Card dealerUpCard)
    {
        if (!Hand.Has2Cards)
            throw new ArgumentException("Invalid number of cards to split");

        if (Hand.Cards.First().Value != Hand.Cards.Last().Value)
            throw new ArgumentException("Hand value is not duplicate");

        if (SplitHands != null)
            return false;

        return Hand.Cards.First().Value switch
        {
            CardValue.Five or CardValue.Ten or CardValue.Jack or CardValue.Queen or CardValue.King => false,
            CardValue.Eight or CardValue.Ace => true,
            CardValue.Four => dealerUpCard.Value is CardValue.Five or CardValue.Six,
            CardValue.Nine => dealerUpCard.Value != CardValue.Seven,
            CardValue.Two or CardValue.Three or CardValue.Seven => 
                CardValueExtensions.InRange(CardValue.Two, CardValue.Seven, dealerUpCard.Value),
            CardValue.Six => CardValueExtensions.InRange(CardValue.Three, CardValue.Six, dealerUpCard.Value),
            _ => throw new NotImplementedException(),
        };
    }
}