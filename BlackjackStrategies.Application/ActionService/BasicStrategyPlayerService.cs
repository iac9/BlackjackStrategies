using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application.ActionService;

public class BasicStrategyPlayerService : BasePlayer
{
    public override HandAction GetAction(Card dealerUpCard)
    {
        HandAction action;
        if (CurrentHand.HasTwoCards)
        {
            if (ShouldSplit(dealerUpCard))
                action = HandAction.Split;
            else if (CurrentHand.Cards.Any(c => c.Value == CardValue.Ace))
                action = GetActionWhenInitialHandHasAce(dealerUpCard);
            else
                action = GetActionWhenInitialHandHasNoAceOrDuplicate(dealerUpCard);
        }
        else
            action = GetActionWhenHandHasNoAceOrDuplicate(dealerUpCard);

        return action;
    }

    private HandAction GetActionWhenInitialHandHasNoAceOrDuplicate(Card dealerUpCard)
    {
        return CurrentHand.GetValue() switch
        {
            9 => dealerUpCard.Value.IsBetween(CardValue.Three, CardValue.Six) ? HandAction.Double : HandAction.Hit,
            10 => dealerUpCard.Value.IsBetween(CardValue.Two, CardValue.Nine) ? HandAction.Double : HandAction.Hit,
            11 => HandAction.Double,
            _ => GetActionWhenHandHasNoAceOrDuplicate(dealerUpCard)
        };
    }
    
    private HandAction GetActionWhenHandHasNoAceOrDuplicate(Card dealerUpCard)
    {
        return CurrentHand.GetValue() switch
        {
            < 9 => HandAction.Hit,
            12 => dealerUpCard.Value.IsBetween(CardValue.Four, CardValue.Six) ? HandAction.Stay : HandAction.Hit,
            >= 13 and <= 16 => dealerUpCard.Value.IsBetween(CardValue.Two, CardValue.Six)
                ? HandAction.Stay
                : HandAction.Hit,
            _ => HandAction.Stay
        };
    }


    private HandAction GetActionWhenInitialHandHasAce(Card dealerUpCard)
    {
        var playerOtherCard = CurrentHand.Cards.FirstOrDefault(c => c.Value != CardValue.Ace) ??
                              CurrentHand.Cards.First();

        return playerOtherCard.Value switch
        {
            CardValue.Two or CardValue.Three => dealerUpCard.Value.IsBetween(CardValue.Five, CardValue.Six)
                ? HandAction.Double
                : HandAction.Hit,
            CardValue.Four or CardValue.Five => dealerUpCard.Value.IsBetween(CardValue.Four, CardValue.Six)
                ? HandAction.Double
                : HandAction.Hit,
            CardValue.Six => dealerUpCard.Value.IsBetween(CardValue.Three, CardValue.Six)
                ? HandAction.Double
                : HandAction.Hit,
            CardValue.Seven => dealerUpCard.Value.IsBetween(CardValue.Two, CardValue.Six) ? HandAction.Double :
                dealerUpCard.Value.IsBetween(CardValue.Seven, CardValue.Eight) ? HandAction.Stay : HandAction.Hit,
            CardValue.Eight => dealerUpCard.Value == CardValue.Six ? HandAction.Double : HandAction.Stay,
            _ => HandAction.Stay,
        };
    }

    private bool ShouldSplit(Card dealerUpCard)
    {
        if (!CurrentHand.HasTwoCards)
            throw new ArgumentException("Invalid number of cards to split");

        var handIsDuplicate = CurrentHand.Cards.First().Value == CurrentHand.Cards.Last().Value;

        return handIsDuplicate && CurrentHand.Cards.First().Value switch
        {
            CardValue.Five or CardValue.Ten or CardValue.Jack or CardValue.Queen or CardValue.King => false,
            CardValue.Eight or CardValue.Ace => true,
            CardValue.Four => dealerUpCard.Value is CardValue.Five or CardValue.Six,
            CardValue.Nine => dealerUpCard.Value != CardValue.Seven,
            CardValue.Two or CardValue.Three or CardValue.Seven =>
                dealerUpCard.Value.IsBetween(CardValue.Two, CardValue.Seven),
            CardValue.Six => dealerUpCard.Value.IsBetween(CardValue.Three, CardValue.Six),
            _ => throw new InvalidOperationException(),
        };
    }
}