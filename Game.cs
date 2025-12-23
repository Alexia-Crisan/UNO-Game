class Game
{
    private Deck deck;
    private List<Player> players;
    private Card topCard;
    private int currentPlayerIndex;

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

        topCard = deck.DrawCard();
        currentPlayerIndex = 0;
    }

    public void Play()
    {
        while (true)
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
                currentPlayer.DrawCard(deck);
            }

            if (currentPlayer.Hand.Count == 0)
            {
                Console.WriteLine($"\n{currentPlayer.Name} wins!");
                break;
            }

            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        }
    }
}