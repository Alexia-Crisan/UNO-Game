class ScoringRules
{
    public static int GetCardScore(Card card)
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

    public static int CalculateRoundScore(List<Player> players, List<Player> winners)
    {
        int score = 0;

        foreach (Player player in players)
        {
            if (winners.Contains(player)) continue;

            foreach (Card card in player.Hand)
                score += GetCardScore(card);
        }

        return score;
    }
}