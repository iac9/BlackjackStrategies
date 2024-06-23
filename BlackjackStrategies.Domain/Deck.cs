namespace BlackjackStrategies.Domain
{
    public class Deck(int deckSize = 1)
    {
        private List<Card> cards = GenerateDeck(deckSize).ToList();

        public int Count { get => cards.Count; }

        public Card Draw()
        {
            if (cards.Count == 0)
                throw new InvalidOperationException("Deck is empty");

            var topCardIndex = cards.Count - 1;
            var topCard = cards[topCardIndex];
            cards.RemoveAt(topCardIndex);

            return topCard;
        }

        public void Shuffle()
        {
             cards = [.. cards.OrderBy(c => Guid.NewGuid())];
        }

        public void ResetDeck(int deckSize) => cards = GenerateDeck(deckSize).ToList();

        private static IEnumerable<Card> GenerateDeck(int deckSize)
        {
            for (int _ = 0; _ < deckSize; _++)
            {
                foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
                {
                    foreach (CardValue value in Enum.GetValues(typeof(CardValue)))
                    {
                        yield return new Card
                        {
                            Suit = suit,
                            Value = value
                        };
                    }
                }
            }
        }
    }
}
