class Deck
{
    private Stack<Card> cards;
    private Random rand = new Random();

    public int Count => cards.Count;

    public Deck()
    {
        List<Card> deckList = new List<Card>();

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

        deckList = deckList.OrderBy(x => rand.Next()).ToList();
        cards = new Stack<Card>(deckList);
    }

    public bool DrawCard(out Card drawnCard)
    {
        if (cards.Count == 0)
        {
            drawnCard = null;
            return false;
        }

        drawnCard = cards.Pop();
        return true;
    }

    public void PutCardBack(Card card)
    {
        cards.Push(card);
    }

}