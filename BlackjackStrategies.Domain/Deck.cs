namespace BlackjackStrategies.Domain
{
    public class Deck
    {
        private List<Card> _cards = [];
        public int Count => _cards.Count;
        public int DeckSize { get; }

        public Deck(int deckSize = 1)
        {
            DeckSize = deckSize;
            ResetDeck();
        }

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


        public void ResetDeck()
        {
            _cards.Clear();

            for (int _ = 0; _ < DeckSize; _++)
            {
                foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
                {
                    foreach (CardValue value in Enum.GetValues(typeof(CardValue)))
                    {
                        _cards.Add(new Card
                        {
                            Suit = suit,
                            Value = value
                        });
                    }
                }
            }
        }
    }
}
