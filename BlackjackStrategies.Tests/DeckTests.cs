using BlackjackStrategies.Domain;
using FluentAssertions;
using System.Reflection;

namespace BlackjackStrategies.Tests
{
    public class DeckTests
    {
        [Fact]
        public void Draw_ThrowsInvalidOperationException_IfNoCardsLeft()
        {
            var deck = new Deck(0);

            Action action = () => deck.Draw();

            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Draw_ReturnAndRemovesTopCard_IfCardsAvailable()
        {
            var deck = new Deck(1);
            Type deckType = typeof(Deck);
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

        [Fact]
        public void ResetDeck_ClearsAllCardsAndGenerateNewDeckWithGivenSize()
        {
            var expectedNumberOfCards = 3 * 52;
            var deck = new Deck(3);
            Type deckType = typeof(Deck);
            var propertyInfo = deckType.GetField("_cards", BindingFlags.NonPublic | BindingFlags.Instance);
            propertyInfo?.SetValue(deck, new List<Card>());

            deck.ResetDeck();

            deck.Count.Should().Be(expectedNumberOfCards);
        }
    }
}
