﻿namespace BlackjackStrategies.Domain;

public class Card
{
    public CardSuit Suit { get; set; }
    public CardValue Value { get; set; }

    public override string ToString()
    {
        return $"[{Suit} {Value}]";
    }
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
    public static bool IsBetween(this CardValue card, CardValue start, CardValue end)
    {
        if (start > end)
            throw new ArgumentException("Start card value must be less than or equal to end card value.");

        var startIndex = (int)start;
        var endIndex = (int)end;
        var cardIndex = (int)card;

        return startIndex <= cardIndex && cardIndex <= endIndex;
    }
}