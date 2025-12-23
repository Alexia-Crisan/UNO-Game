// See https://aka.ms/new-console-template for more information
Console.WriteLine("========== UNO Game ==========");

List<string> playerNames = new List<string> { "Alice", "Bob", "Charlie" };
Game game = new Game(playerNames);
game.Play();
