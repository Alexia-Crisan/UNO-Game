class CardEffectManager
{
    public static int ApplyCardEffect(Card card, Game game)
    {
        switch (card.Type)
        {
            case CardType.SkipTurn:
                Console.WriteLine("[SKIP] Next player is skipped!");
                return 2;

            case CardType.ReverseOrder:
                Console.WriteLine("[REVERSE] Direction reversed!");
                game.ReverseDirection();
                return 1;

            case CardType.DrawTwoCards:
                var nextPlayer = game.GetNextPlayerIndex();
                Console.WriteLine($"[+2] {game.Players[nextPlayer].Name} draws 2 cards!");
                game.Players[nextPlayer].DrawCard(game.Deck, game.DiscardPile, null);
                game.Players[nextPlayer].DrawCard(game.Deck, game.DiscardPile, null);
                return 2;

            case CardType.WildCard:
                var color = game.ChooseColor(game.CurrentPlayer);
                Console.WriteLine($"[COLOR CHANGE] Wild card played! Color changed to {color}");
                card.SetColor(color);
                return 1;

            case CardType.WildDrawFour:
                var color4 = game.ChooseColor(game.CurrentPlayer);
                Console.WriteLine($"[+4] Wild Draw Four! Color changed to {color4}");
                card.SetColor(color4);
                var next = game.GetNextPlayerIndex();
                Console.WriteLine($"[+4] {game.Players[next].Name} draws 4 cards!");
                for (int i = 0; i < 4; i++)
                    game.Players[next].DrawCard(game.Deck, game.DiscardPile, null);
                return 2;

            default:
                return 1;
        }
    }
}