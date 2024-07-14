namespace BlackjackStrategies.Domain
{
    public class Deck
    {
        private List<Card> _cards = [];
        public int Count => _cards.Count;
        public int NumberOfDecks { get; }

        public Deck(int numberOfDecks = 1)
        {
            NumberOfDecks = numberOfDecks;
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

            for (int _ = 0; _ < NumberOfDecks; _++)
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
