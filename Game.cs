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
    private ScoreManager scoreManager;


    public List<Player> Players => players;
    public Deck Deck => deck;
    public List<Card> DiscardPile => discardPile;
    public Player CurrentPlayer => players[currentPlayerIndex];
    public int CurrentPlayerIndex { get => currentPlayerIndex; set => currentPlayerIndex = value; }

    public void ReverseDirection() => direction *= -1;

    public Game(List<string> playerNames)
    {
        deck = new Deck();
        players = new List<Player>();

        foreach (string name in playerNames)
        {
            Player p = new Player(name);
            players.Add(p);
        }

        scoreManager = new ScoreManager(players);

        Deal7Cards();

        if (!deck.DrawCard(out topCard, discardPile, null))
        {
            playing = false;
            return;
        }

        currentPlayerIndex = 0;
    }

    private void Deal7Cards()
    {
        foreach (var player in players)
            for (int i = 0; i < 7; i++)
                player.DrawCard(deck, discardPile, null);
    }

    public int GetNextPlayerIndex(int steps = 1)
    {
        int index = currentPlayerIndex;

        for (int i = 0; i < steps; i++)
            index = (index + direction + players.Count) % players.Count;

        return index;
    }

    public Color ChooseColor(Player player)
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

    public void PlayOneRound()
    {
        while (playing == true)
        {
            Player currentPlayer = players[currentPlayerIndex];

            Console.Write("\n[TOP CARD]: "); topCard.PrintColored(); Console.WriteLine();

            currentPlayer.ShowHand2();

            Card playedCard = currentPlayer.PlayCard(topCard);

            if (playedCard != null)
            {
                Console.WriteLine($"[PLAYED CARD] {currentPlayer.Name} plays {playedCard}");
                Console.WriteLine();

                // reset color for wild card + add to discard pile
                if (topCard.Type == CardType.WildCard || topCard.Type == CardType.WildDrawFour)
                    topCard.SetColor(Color.None);
                discardPile.Add(topCard);
                topCard = playedCard;

                currentPlayerIndex = GetNextPlayerIndex(CardEffectManager.ApplyCardEffect(playedCard, this));
            }
            else
            {
                Console.WriteLine($"[DRAW] {currentPlayer.Name} has no playable card. Drawing one...");

                bool drewCard = currentPlayer.DrawCard(deck, discardPile, topCard);
                if (!drewCard)
                {
                    Console.WriteLine("\n[END] No cards left anywhere! Game ends!");
                    EndGameByFewestCards();
                    playing = false;
                    return;
                }

                currentPlayerIndex = GetNextPlayerIndex();
            }

            if (currentPlayer.Hand.Count == 0)
            {
                Console.WriteLine($"\n[WIN] {currentPlayer.Name} wins!");
                scoreManager.AwardPoints(new List<Player> { currentPlayer });
                playing = false;
                return;
            }

            int totalInHands = players.Sum(p => p.Hand.Count);
            if (totalInHands == 108)
            {
                Console.WriteLine("\n[END] All cards are in players' hands! Game ends!");
                EndGameByFewestCards();
                return;
            }
        }
    }

    private void EndGameByFewestCards()
    {
        int minCards = players.Min(p => p.Hand.Count);
        var winners = players.Where(p => p.Hand.Count == minCards).ToList();

        if (winners.Count == 1)
        {
            Console.WriteLine($"\n[END] Game ends! {winners[0].Name} wins with {minCards} cards left!");
        }
        else
        {
            Console.WriteLine($"\n[END] Game ends! Tie between: {string.Join(", ", winners.Select(p => p.Name))}");
        }

        scoreManager.AwardPoints(winners);
        playing = false;
    }

    private void ResetRound()
    {
        deck = new Deck();
        discardPile.Clear();

        foreach (Player player in players)
            player.Hand.Clear();

        Deal7Cards();

        deck.DrawCard(out topCard, discardPile, null);
        currentPlayerIndex = 0;
        direction = 1;
        playing = true;
    }

    public void PlayTournament()
    {
        Console.WriteLine("════════════════════════════════════");
        Console.WriteLine("║           MATCH START             ║");
        Console.WriteLine("════════════════════════════════════");


        while (true)
        {
            ResetRound();
            PlayOneRound();

            if (scoreManager.HasWinner(out Player matchWinner))
            {
                Console.WriteLine($"\n[MATCH WINNER]: {matchWinner.Name} with {scoreManager.GetScore(matchWinner)} points!");
                scoreManager.PrintScores();
                break;
            }

            Console.WriteLine("\n----------------------------------------");
            Console.WriteLine("|         NEXT ROUND STARTING          |");
            Console.WriteLine("----------------------------------------");

            Console.ReadLine();
        }
    }

}