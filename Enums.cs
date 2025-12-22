public enum Color { Red, Blue, Green, Yellow, None }
public enum CardType
{
    Number,         // Normal numbered card (0-9)
    SkipTurn,       // Skip the next players turn
    ReverseOrder,   // Reverse the turn order
    DrawTwoCards,   // Next player draws 2 cards
    WildCard,       // Player chooses the color
    WildDrawFour    // Player chooses color and next player draws 4
}
