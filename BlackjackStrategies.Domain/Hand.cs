﻿namespace BlackjackStrategies.Domain
{
    public class Hand {
        public Hand(params Card[] cards)
        {
            Cards = [.. cards];
        }

        public List<Card> Cards { get; } 
        public void AddCard(Card card) => Cards.Add(card);
        public void Clear() => Cards.Clear();
        public bool Has2Cards => Cards.Count == 2;
        public int GetValue()
        {
            var total = 0;
            var numberOfAces = 0;

            foreach (var card in Cards)
            {
                if (card.Value != CardValue.Ace)
                {
                    total += card.Value switch
                    {
                        CardValue.Two => 2,
                        CardValue.Three => 3,
                        CardValue.Four => 4,
                        CardValue.Five => 5,
                        CardValue.Six => 6,
                        CardValue.Seven => 7,
                        CardValue.Eight => 8,
                        CardValue.Nine => 9,
                        _ => 10,
                    };
                }
                else
                    numberOfAces++;

            }

            while (numberOfAces > 0)
            {
                total += total > 10 ? 1 : 11;
                numberOfAces--;
            }

            return total;
        }

        public override string ToString() => 
            string.Join(" ", Cards.Select(c => c.ToString()));
    }

    public static class HandExtensions
    {
        public static GameResult GetGameResult(this Hand hand, Hand otherHand)
        {
            var playerHandValue = hand.GetValue();
            var otherHandValue = otherHand.GetValue();

            if (playerHandValue > 21)
                return GameResult.Lose;
            else
            {
                if (playerHandValue == 21)
                    return GameResult.Blackjack;

                if (otherHandValue > 21)
                    return GameResult.Win;

                if (playerHandValue == otherHandValue)
                    return GameResult.Push;
                else if (playerHandValue < otherHandValue)
                    return GameResult.Lose;
                else
                    return GameResult.Win;
            }
        }
    }
}
