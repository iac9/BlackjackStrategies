namespace BlackjackStrategies.Domain
{
    public class Deck(int deckSize = 1)
    {
        private List<Card> _cards = GenerateDeck(deckSize).ToList();

        public int Count { get => _cards.Count; }

        public Card Draw()
        {
            if (_cards.Count == 0)
                throw new InvalidOperationException("Deck is empty");

            var topCardIndex = _cards.Count - 1;
            var topCard = _cards[topCardIndex];
            _cards.RemoveAt(topCardIndex);

            return topCard;
        }

        public void Shuffle() => 
            _cards = [.. _cards.OrderBy(c => Guid.NewGuid())];

        public void ResetDeck() => 
            _cards = GenerateDeck(deckSize).ToList();

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
