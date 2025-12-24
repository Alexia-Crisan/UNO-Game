// See https://aka.ms/new-console-template for more information
Console.WriteLine("========== UNO Game ==========");

List<string> playerNames = new List<string> { "Alice", "Bob", "Charlie", "Emanuela", "Greg", "Cerasela" };
Game game = new Game(playerNames);
game.PlayTournament();
