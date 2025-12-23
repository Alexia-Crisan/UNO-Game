class Game
{
    private Deck deck;
    private List<Player> players;
    private Card topCard;
    private int currentPlayerIndex;

    private int direction = 1; // 1 = clockwise, -1 = counter-clockwise

    private bool playing = true;

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
                player.DrawCard(deck);


        if (!deck.DrawCard(out topCard))
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

    public void Play()
    {
        while (playing == true)
        {
            Player currentPlayer = players[currentPlayerIndex];
            Console.WriteLine($"\nTop card: {topCard}");
            currentPlayer.ShowHand();

            Card playedCard = currentPlayer.PlayCard(topCard);

            if (playedCard != null)
            {
                Console.WriteLine($"{currentPlayer.Name} plays {playedCard}");
                topCard = playedCard;
            }
            else
            {
                Console.WriteLine($"{currentPlayer.Name} has no playable card. Drawing one...");

                bool cardDrawn = currentPlayer.DrawCard(deck);

                if (!cardDrawn)
                {
                    Console.WriteLine("\nDeck is empty! Game ends!");
                    playing = false;
                    break;
                }
            }

            if (currentPlayer.Hand.Count == 0)
            {
                Console.WriteLine($"\n{currentPlayer.Name} wins!");
                break;
            }

            currentPlayerIndex = GetNextPlayerIndex(1);
        }
    }
}