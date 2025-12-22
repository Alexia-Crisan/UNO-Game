class Player
{
    public string Name { get; private set; }
    public List<Card> Hand { get; private set; } = new List<Card>();

    public Player(string name)
    {
        Name = name;
    }

    public void DrawCard(Deck deck)
    {
        Hand.Add(deck.DrawCard());
    }

    public Card PlayCard(Card topCard)
    {
        for (int i = 0; i < Hand.Count; i++)
        {
            if (Hand[i].isPlayable(topCard))
            {
                Card playable = Hand[i];
                Hand.RemoveAt(i);
                return playable;
            }
        }

        // no playable card found
        return null;
    }

    public void ShowHand()
    {
        Console.WriteLine($"{Name}'s hand:");
        for (int i = 0; i < Hand.Count; i++)
            Console.WriteLine($"{i}: {Hand[i]}");
    }
}