using System;
using System.Globalization;

namespace BlackjackStrategies.Domain
{
    public class Card
    {
        public CardSuit Suit { get; set; }
        public CardValue Value { get; set; }
        public override string ToString() => $"[{Suit} {Value}]";
    }

    public enum CardSuit
    {
        Spades,
        Clubs,
        Diamonds,
        Hearts
    }

    public enum CardValue
    {
        Two,
        Three, 
        Four,
        Five,
        Six, 
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }

    public static class CardValueExtensions
    {
        public static bool InRange(CardValue start, CardValue end, CardValue card)
        {
            if (start > end)
                throw new ArgumentException("Start card value must be less than or equal to end card value.");

            int startIndex = (int)start;
            int endIndex = (int)end;
            int cardIndex = (int)card;

            return startIndex <= cardIndex && cardIndex <= endIndex;
        }
    }
}
