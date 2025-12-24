class Round
{
    private Deck deck;
    private List<Player> players;
    private Card topCard;
    private int currentPlayerIndex;
    private int direction = 1; // 1 = clockwise, -1 = counter-clockwise
    private bool playing = true;
    private List<Card> discardPile = new List<Card>();
    public bool IsFinished { get; private set; } = false;


    public List<Player> Players => players;
    public Deck Deck => deck;
    public List<Card> DiscardPile => discardPile;
    public Player CurrentPlayer => players[currentPlayerIndex];

    public void ReverseDirection() => direction *= -1;

    public Round(List<Player> players)
    {
        this.players = players;
        deck = new Deck();
        discardPile = new List<Card>();

        Deal7Cards();
        deck.DrawCard(out topCard, discardPile, null);
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

    public List<Player> PlayOneRound()
    {
        while (!IsFinished)
        {
            Player currentPlayer = CurrentPlayer;
            currentPlayer.ShowHand2();

            Console.Write("\n[TOP CARD]: "); topCard.PrintColored(); Console.WriteLine();

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

                int steps = CardEffectManager.ApplyCardEffect(playedCard, this);
                currentPlayerIndex = GetNextPlayerIndex(steps);
            }
            else
            {
                Console.WriteLine($"[DRAW] {currentPlayer.Name} has no playable card. Drawing one...");

                bool drewCard = currentPlayer.DrawCard(deck, discardPile, topCard);
                if (!drewCard)
                {
                    Console.WriteLine("\n[END] No cards left anywhere! Game ends!");
                    IsFinished = true;
                    return DetermineWinners();
                }

                currentPlayerIndex = GetNextPlayerIndex();
            }

            if (currentPlayer.Hand.Count == 0)
            {
                IsFinished = true;
                Console.WriteLine($"\n[WIN] {currentPlayer.Name} wins!");
                return new List<Player> { currentPlayer };
            }

            int totalInHands = players.Sum(p => p.Hand.Count);
            if (totalInHands == 108)
            {
                Console.WriteLine("\n[END] All cards are in players' hands! Game ends!");
                return DetermineWinners(); ;
            }
        }

        return new List<Player>();
    }

    private List<Player> DetermineWinners()
    {
        int minCards = players.Min(p => p.Hand.Count);
        return players.Where(p => p.Hand.Count == minCards).ToList();
    }
}