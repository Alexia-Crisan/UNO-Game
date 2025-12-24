class ScoreManager
{
    private List<Player> players;
    private List<int> scores;

    public const int WINNING_SCORE = 600;

    public ScoreManager(List<Player> players)
    {
        this.players = players;
        scores = new List<int>(new int[players.Count]);
    }

    public void AwardPoints(List<Player> winners)
    {
        int roundScore = ScoringRules.CalculateRoundScore(players, winners);

        foreach (Player winner in winners)
        {
            int index = players.IndexOf(winner);
            scores[index] += roundScore;
        }

        Console.WriteLine($"\n[SCORE] Points awarded: {roundScore}");
    }

    public bool HasWinner(out Player winner)
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

    public void PrintScores()
    {
        Console.WriteLine("\n++======================++");
        Console.WriteLine("||      SCOREBOARD      ||");
        Console.WriteLine("++======================++");

        for (int i = 0; i < players.Count; i++)
        {
            Console.WriteLine($"{players[i].Name}: {scores[i]} pts");
        }
    }

    public int GetScore(Player player)
    {
        int index = players.IndexOf(player);
        return index >= 0 ? scores[index] : 0;
    }
}