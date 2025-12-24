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

    private List<int> scores;

    private const int WINNING_SCORE = 600;


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
        foreach (Player player in players)
            for (int i = 0; i < 7; i++)
                player.DrawCard(deck, discardPile, null);


        if (!deck.DrawCard(out topCard, discardPile, null))
        {
            playing = false;
            return;
        }

        currentPlayerIndex = 0;

        scores = new List<int>();
        for (int i = 0; i < players.Count; i++)
            scores.Add(0);

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

    public void PlayOneRound()
    {
        while (playing == true)
        {
            Player currentPlayer = players[currentPlayerIndex];

            Console.Write("\n[Top card]: ");
            topCard.PrintColored();
            Console.WriteLine();

            currentPlayer.ShowHand2();

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
                AwardPoints(new List<Player> { currentPlayer });
                playing = false;
                return;
            }

            int totalInHands = players.Sum(p => p.Hand.Count);
            if (totalInHands == 108)
            {
                Console.WriteLine("\nAll cards are in players' hands! Game ends!");
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
            Console.WriteLine($"\nGame ends! {winners[0].Name} wins with {minCards} cards left!");
        }
        else
        {
            Console.WriteLine($"\nGame ends! Tie between: {string.Join(", ", winners.Select(p => p.Name))}");
        }

        AwardPoints(winners);
        playing = false;
    }

    private int GetCardScore(Card card)
    {
        return card.Type switch
        {
            CardType.Number => 10,
            CardType.SkipTurn => 25,
            CardType.ReverseOrder => 25,
            CardType.DrawTwoCards => 25,
            CardType.WildCard => 30,
            CardType.WildDrawFour => 40,
            _ => 0
        };
    }
    private int CalculateScoreForWinners(List<Player> winners)
    {
        int score = 0;

        foreach (Player player in players)
        {
            if (winners.Contains(player))
                continue;

            foreach (Card card in player.Hand)
            {
                score += GetCardScore(card);
            }
        }

        return score;
    }

    private void PrintScores()
    {
        Console.WriteLine("\n=== SCOREBOARD ===");
        for (int i = 0; i < players.Count; i++)
        {
            Console.WriteLine($"{players[i].Name}: {scores[i]} pts");
        }
    }

    private void AwardPoints(List<Player> winners)
    {
        int roundScore = CalculateScoreForWinners(winners);

        foreach (Player winner in winners)
        {
            int index = players.IndexOf(winner);
            scores[index] += roundScore;
        }

        Console.WriteLine($"\nPoints awarded: {roundScore}");
    }

    private bool HasMatchWinner(out Player winner)
    {
        for (int i = 0; i < scores.Count; i++)
        {
            if (scores[i] >= WINNING_SCORE)
            {
                winner = players[i];
                return true;
            }
        }

        winner = null;
        return false;
    }

    private void ResetRound()
    {
        deck = new Deck();
        discardPile.Clear();

        foreach (Player player in players)
            player.Hand.Clear();

        foreach (Player player in players)
            for (int i = 0; i < 7; i++)
                player.DrawCard(deck, discardPile, null);

        deck.DrawCard(out topCard, discardPile, null);
        currentPlayerIndex = 0;
        direction = 1;
        playing = true;
    }

    public void PlayTournament()
    {
        Console.WriteLine("\n=== MATCH START ===");

        while (true)
        {
            ResetRound();
            PlayOneRound();

            if (HasMatchWinner(out Player matchWinner))
            {
                Console.WriteLine($"\n[MATCH WINNER]: {matchWinner.Name} with {scores[players.IndexOf(matchWinner)]} points!");
                PrintScores();
                break;
            }

            Console.WriteLine("\n--- Next round starting ---");
            Console.ReadLine();
        }
    }

}