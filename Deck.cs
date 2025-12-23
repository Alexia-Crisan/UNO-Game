using System.Runtime.InteropServices.Marshalling;

class Deck
{
    private Stack<Card> cards;

    private List<Card> deckList;

    private static Random rand = new Random();

    public int Count => cards.Count;

    public Deck()
    {
        deckList = new List<Card>();

        // add coloured cards of all types
        foreach (Color color in Enum.GetValues(typeof(Color)))
        {
            if (color == Color.None) continue;

            for (int i = 0; i <= 9; i++)
                deckList.Add(new Card(color, CardType.Number, i));

            for (int j = 0; j < 2; j++)
            {
                deckList.Add(new Card(color, CardType.SkipTurn));
                deckList.Add(new Card(color, CardType.ReverseOrder));
                deckList.Add(new Card(color, CardType.DrawTwoCards));
            }
        }

        // add wild cards
        for (int i = 0; i < 4; i++)
        {
            deckList.Add(new Card(Color.None, CardType.WildCard));
            deckList.Add(new Card(Color.None, CardType.WildDrawFour));
        }

        Shuffle();
    }

    public bool DrawCard(out Card card, List<Card> discardPile, Card topCard)
    {
        if (cards.Count == 0)
        {
            if (discardPile.Count == 0)
            {
                card = null;
                return false; // no cards left anywhere
            }

            RefillFromDiscard(discardPile, topCard);
        }

        card = cards.Pop();
        return true;
    }

    public void PutCardBack(Card card)
    {
        cards.Push(card);
    }

    public void Shuffle()
    {
        deckList = deckList.OrderBy(x => rand.Next()).ToList();
        cards = new Stack<Card>(deckList);
    }

    public void RefillFromDiscard(List<Card> discardPile, Card topCard)
    {
        Console.WriteLine("========== [RefillFromDiscard]");
        if (discardPile == null || discardPile.Count == 0)
            return;

        cards.Clear();

        deckList = discardPile.ToList();

        if (topCard != null)
            deckList.Remove(topCard);

        discardPile.Clear();

        Shuffle();
    }
}