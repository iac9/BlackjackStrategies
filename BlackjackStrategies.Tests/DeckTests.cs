using System.Reflection;
using BlackjackStrategies.Domain;
using BlackjackStrategies.Domain.Deck;
using FluentAssertions;

namespace BlackjackStrategies.Tests;

public class DeckTests
{
    [Fact]
    public void Draw_ThrowsInvalidOperationException_IfNoCardsLeft()
    {
        var deck = new Deck();

        Action action = () => deck.Draw();

        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Draw_ReturnAndRemovesTopCard_IfCardsAvailable()
    {
        var deck = new Deck();
        var deckType = typeof(Deck);
        var propertyInfo = deckType.GetField("_cards", BindingFlags.NonPublic | BindingFlags.Instance);
        propertyInfo?.SetValue(deck, new List<Card>
        {
            new() { Suit = CardSuit.Spades, Value = CardValue.Ace }
        });

        var actualDrawnCard = deck.Draw();

        actualDrawnCard.Suit.Should().Be(CardSuit.Spades);
        actualDrawnCard.Value.Should().Be(CardValue.Ace);
        deck.Count.Should().Be(0);
    }
}