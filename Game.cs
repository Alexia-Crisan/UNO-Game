using System.Globalization;

class Game
{
    private Deck deck;
    private List<Player> players;
    private Card topCard;
    private int currentPlayerIndex;

    private int direction = 1; // 1 = clockwise, -1 = counter-clockwise

    private bool playing = true;

    private List<Card> discardPile = new List<Card>();

    public Game(List<string> playerNames)
    {
        deck = new Deck();
        players = new List<Player>();

        foreach (string name in playerNames)
        {
            Player p = new Player(name);
            players.Add(p);
        }

        // deal 7 cards to each player
        foreach (var player in players)
            for (int i = 0; i < 7; i++)
                player.DrawCard(deck, discardPile, null);


        if (!deck.DrawCard(out topCard, discardPile, null))
        {
            playing = false;
            return;
        }
        currentPlayerIndex = 0;
    }

    private int GetNextPlayerIndex(int steps = 1)
    {
        int index = currentPlayerIndex;

        for (int i = 0; i < steps; i++)
        {
            index = (index + direction + players.Count) % players.Count;
        }

        return index;
    }

    private Color ChooseColor(Player player)
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

    private void ApplyCardEffect(Card card)
    {
        switch (card.Type)
        {
            case CardType.SkipTurn:
                {
                    Console.WriteLine("Next player is skipped!");
                    currentPlayerIndex = GetNextPlayerIndex(2);
                    break;
                }

            case CardType.ReverseOrder:
                {
                    Console.WriteLine("Direction reversed!");
                    direction *= -1;
                    currentPlayerIndex = GetNextPlayerIndex();
                    break;
                }

            case CardType.DrawTwoCards:
                {
                    int nextPlayer = GetNextPlayerIndex();
                    Console.WriteLine($"{players[nextPlayer].Name} draws 2 cards!");
                    players[nextPlayer].DrawCard(deck, discardPile, null);
                    players[nextPlayer].DrawCard(deck, discardPile, null);
                    currentPlayerIndex = GetNextPlayerIndex(2);
                    break;
                }

            case CardType.WildCard:
                {
                    Color chosenColor = ChooseColor(players[currentPlayerIndex]);
                    Console.WriteLine($"[W] Wild card played! Color changed to {chosenColor}");
                    card.SetColor(chosenColor);

                    currentPlayerIndex = GetNextPlayerIndex();
                    break;
                }

            case CardType.WildDrawFour:
                {
                    Color chosenColor = ChooseColor(players[currentPlayerIndex]);
                    Console.WriteLine($"[W] Wild Draw Four! Color changed to {chosenColor}");
                    card.SetColor(chosenColor);

                    int nextPlayer4 = GetNextPlayerIndex();
                    Console.WriteLine($"{players[nextPlayer4].Name} draws 4 cards!");
                    for (int i = 0; i < 4; i++)
                        players[nextPlayer4].DrawCard(deck, discardPile, null);
                    currentPlayerIndex = GetNextPlayerIndex(2);
                    break;
                }

            default:
                {
                    currentPlayerIndex = GetNextPlayerIndex();
                    break;
                }
        }
    }

    public void Play()
    {
        while (playing == true)
        {
            Player currentPlayer = players[currentPlayerIndex];

            Console.Write("\n[Top card]: ");
            topCard.PrintColored();
            Console.WriteLine();

            currentPlayer.ShowHand();

            Card playedCard = currentPlayer.PlayCard(topCard);

            if (playedCard != null)
            {
                Console.WriteLine($"{currentPlayer.Name} plays {playedCard}");
                Console.WriteLine();

                // reset color for wild card + add to discard pile
                if (topCard.Type == CardType.WildCard || topCard.Type == CardType.WildDrawFour)
                    topCard.SetColor(Color.None);
                discardPile.Add(topCard);
                topCard = playedCard;

                ApplyCardEffect(playedCard);
            }
            else
            {
                Console.WriteLine($"{currentPlayer.Name} has no playable card. Drawing one...");

                bool drewCard = currentPlayer.DrawCard(deck, discardPile, topCard);
                if (!drewCard)
                {
                    Console.WriteLine("\nNo cards left anywhere! Game ends!");
                    EndGameByFewestCards();
                    playing = false;
                    return;
                }

                currentPlayerIndex = GetNextPlayerIndex(1);
            }

            if (currentPlayer.Hand.Count == 0)
            {
                Console.WriteLine($"\n{currentPlayer.Name} wins!");
                break;
            }

            int totalInHands = players.Sum(p => p.Hand.Count);
            if (totalInHands == 108)
            {
                Console.WriteLine("\nAll cards are in players' hands! Game ends!");
                EndGameByFewestCards();
                break;
            }
        }
    }

    private void EndGameByFewestCards()
    {
        int minCards = players.Min(p => p.Hand.Count);
        var winners = players.Where(p => p.Hand.Count == minCards).ToList();

        if (winners.Count == 1)
        {
            Console.WriteLine($"\nGame ends! {winners[0].Name} wins with {minCards} cards left!");
        }
        else
        {
            Console.WriteLine($"\nGame ends! It's a tie between: {string.Join(", ", winners.Select(p => p.Name))} with {minCards} cards left each.");
        }

        playing = false;
    }

}