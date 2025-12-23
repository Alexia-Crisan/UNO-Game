class Player
{
    public string Name { get; private set; }
    public List<Card> Hand { get; private set; } = new List<Card>();

    public Player(string name)
    {
        Name = name;
    }

    public bool DrawCard(Deck deck)
    {
        Card drawnCard;

        if (!deck.DrawCard(out drawnCard))
            return false; // empty deck

        Hand.Add(drawnCard);
        return true;
    }

    public Card PlayCard(Card topCard)
    {
        Random rand = new Random();

        // 1. Try to play a number card
        for (int i = 0; i < Hand.Count; i++)
        {
            if (Hand[i].Type == CardType.Number && Hand[i].IsPlayable(topCard))
            {
                Card playable = Hand[i];
                Hand.RemoveAt(i);
                return playable;
            }
        }

        // 2. Play a colored special card (SkipTurn, ReverseOrder, DrawTwoCards)
        List<int> coloredSpecialIndices = new List<int>();
        for (int i = 0; i < Hand.Count; i++)
        {
            if ((Hand[i].Type == CardType.SkipTurn || Hand[i].Type == CardType.ReverseOrder || Hand[i].Type == CardType.DrawTwoCards) && Hand[i].IsPlayable(topCard))
            {
                coloredSpecialIndices.Add(i);
            }
        }

        if (coloredSpecialIndices.Count > 0)
        {
            int chosenIndex = coloredSpecialIndices[rand.Next(coloredSpecialIndices.Count)];
            Card playable = Hand[chosenIndex];
            Hand.RemoveAt(chosenIndex);
            return playable;
        }

        // 3. Play Wild cards (WildCard)
        List<int> wildIndices = new List<int>();
        for (int i = 0; i < Hand.Count; i++)
        {
            if (Hand[i].Type == CardType.WildCard)
                wildIndices.Add(i);
        }

        if (wildIndices.Count > 0)
        {
            int chosenIndex = wildIndices[rand.Next(wildIndices.Count)];
            Card playable = Hand[chosenIndex];
            Hand.RemoveAt(chosenIndex);
            return playable;
        }

        // 4. Play WildDrawFour cards
        List<int> wildDrawFourIndices = new List<int>();
        for (int i = 0; i < Hand.Count; i++)
        {
            if (Hand[i].Type == CardType.WildDrawFour)
                wildDrawFourIndices.Add(i);
        }

        if (wildDrawFourIndices.Count > 0)
        {
            int chosenIndex = wildDrawFourIndices[rand.Next(wildDrawFourIndices.Count)];
            Card playable = Hand[chosenIndex];
            Hand.RemoveAt(chosenIndex);
            return playable;
        }

        // 5. No playable card
        return null;
    }

    public void ShowHand()
    {
        Console.WriteLine($"{Name}'s hand:");
        for (int i = 0; i < Hand.Count; i++)
            Console.WriteLine($"{i}: {Hand[i]}");
    }
}