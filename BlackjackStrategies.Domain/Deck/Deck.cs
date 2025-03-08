namespace BlackjackStrategies.Domain.Deck;

public class Deck
{
    private List<Card> _cards;

    public Deck()
    {
        _cards = new List<Card>();
    }

    public Deck(IEnumerable<Card> cards)
    {
        _cards = cards.ToList();
    }

    public int Count => _cards.Count;

    public Card Draw()
    {
        if (_cards.Count == 0)
            throw new InvalidOperationException("Deck is empty");

        var topCardIndex = _cards.Count - 1;
        var topCard = _cards[topCardIndex];
        _cards.RemoveAt(topCardIndex);

        return topCard;
    }

    public void Shuffle()
    {
        _cards = [.. _cards.OrderBy(c => Guid.NewGuid())];
    }
}