using System.Globalization;

class Game
{
    private List<Player> players;
    private ScoreManager scoreManager;

    public Game(List<string> playerNames)
    {
        players = new List<Player>();

        foreach (string name in playerNames)
        {
            Player p = new Player(name);
            players.Add(p);
        }

        scoreManager = new ScoreManager(players);
    }

    public void PlayTournament()
    {
        Console.WriteLine("════════════════════════════════════");
        Console.WriteLine("║           MATCH START             ║");
        Console.WriteLine("════════════════════════════════════\n");


        while (true)
        {
            Round round = new Round(players);
            var winners = round.PlayOneRound();

            scoreManager.AwardPoints(winners);

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