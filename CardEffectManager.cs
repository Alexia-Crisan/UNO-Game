class CardEffectManager
{
    public static int ApplyCardEffect(Card card, Round round)
    {
        switch (card.Type)
        {
            case CardType.SkipTurn:
                Console.WriteLine("[SKIP] Next player is skipped!");
                return 2;

            case CardType.ReverseOrder:
                Console.WriteLine("[REVERSE] Direction reversed!");
                round.ReverseDirection();
                return 1;

            case CardType.DrawTwoCards:
                var nextPlayer = round.GetNextPlayerIndex();
                Console.WriteLine($"[+2] {round.Players[nextPlayer].Name} draws 2 cards!");
                round.Players[nextPlayer].DrawCard(round.Deck, round.DiscardPile, null);
                round.Players[nextPlayer].DrawCard(round.Deck, round.DiscardPile, null);
                return 2;

            case CardType.WildCard:
                var color = ChooseColor(round.CurrentPlayer);
                Console.WriteLine($"[COLOR CHANGE] Wild card played! Color changed to {color}");
                card.SetColor(color);
                return 1;

            case CardType.WildDrawFour:
                var color4 = ChooseColor(round.CurrentPlayer);
                Console.WriteLine($"[+4] Wild Draw Four! Color changed to {color4}");
                card.SetColor(color4);
                var next = round.GetNextPlayerIndex();
                Console.WriteLine($"[+4] {round.Players[next].Name} draws 4 cards!");
                for (int i = 0; i < 4; i++)
                    round.Players[next].DrawCard(round.Deck, round.DiscardPile, null);
                return 2;

            default:
                return 1;
        }
    }

    private static Color ChooseColor(Player player)
    {
        int[] colorCount = new int[4]; // Red, Blue, Green, Yellow 

        foreach (Card card in player.Hand)
        {
            if (card.Color != Color.None)
                colorCount[(int)card.Color]++;
        }

        int maxIndex = 0;
        for (int i = 1; i < 4; i++)
        {
            if (colorCount[i] > colorCount[maxIndex])
                maxIndex = i;
        }

        return (Color)maxIndex;
    }
}