namespace BlackjackStrategies.Domain.Deck;

public interface IDeckFactory
{
    Deck CreateDeck();
}

public class DeckFactory : IDeckFactory
{
    public int NumberOfDecks { get; set; }

    public Deck CreateDeck()
    {
        var cards = new List<Card>();

        for (var _ = 0; _ < NumberOfDecks; _++)
            foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
            foreach (CardValue value in Enum.GetValues(typeof(CardValue)))
                cards.Add(new Card
                {
                    Suit = suit,
                    Value = value
                });

        return new Deck(cards);
    }
}