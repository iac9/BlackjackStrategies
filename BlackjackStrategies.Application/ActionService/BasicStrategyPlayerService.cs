using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application.Strategies
{

    public class BasicStrategyPlayerService() : IPlayerService
    {
        public Hand Hand { get; set; } = new();
        public IEnumerable<Hand>? SplitHands { get; set; }

        public HandAction GetAction(Hand playerHand, Card dealerUpCard)
        {
            if (playerHand.Has2Cards)
            {
                if (playerHand.Cards.First().Value == playerHand.Cards.Last().Value &&
                    ShouldSplit(playerHand, dealerUpCard))
                {
                    return HandAction.Split;
                }
                else if (playerHand.Cards.Any(c => c.Value == CardValue.Ace))
                {
                    return HandHasAce(playerHand, dealerUpCard);
                }
            }
            return HandHasNoAce(playerHand, dealerUpCard);
        }

        private static HandAction HandHasNoAce(Hand playerHand, Card dealerUpCard)
        {
            var playerHandValue = playerHand.GetValue();

            if (playerHand.Has2Cards)
            {
                if (playerHandValue == 9)
                    return CardValueExtensions.InRange(CardValue.Three, CardValue.Six, dealerUpCard.Value) ?
                        HandAction.Double : HandAction.Hit;

                if (playerHandValue == 10)
                    return CardValueExtensions.InRange(CardValue.Two, CardValue.Nine, dealerUpCard.Value) ?
                        HandAction.Double : HandAction.Hit;

                if (playerHandValue == 11)
                    return HandAction.Double;
            }

            if (playerHandValue < 9)
                return HandAction.Hit;

            if (playerHandValue == 12)
                return CardValueExtensions.InRange(CardValue.Four, CardValue.Six, dealerUpCard.Value) ?
                    HandAction.Stay : HandAction.Hit;

            if (13 <= playerHandValue && playerHandValue <= 16)
                return CardValueExtensions.InRange(CardValue.Two, CardValue.Six, dealerUpCard.Value) ?
                    HandAction.Stay : HandAction.Hit;

            return HandAction.Stay;
        }



        private static HandAction HandHasAce(Hand playerHand, Card dealerUpCard)
        {
            var playerOtherCard = playerHand.Cards.FirstOrDefault(c => c.Value != CardValue.Ace) ?? playerHand.Cards.First();

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

        private bool ShouldSplit(Hand hand, Card dealerUpCard)
        {
            if (!hand.Has2Cards)
                throw new ArgumentException("Invalid number of cards to split");

            if (hand.Cards.First().Value != hand.Cards.Last().Value)
                throw new ArgumentException("Hand value is not duplicate");

            if (SplitHands != null)
                return false;

            return hand.Cards.First().Value switch
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
}
