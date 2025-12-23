using System.Net;

class Card
{
    public Color Color { get; private set; }
    public CardType Type { get; private set; }
    public int Number { get; private set; }

    public Card(Color color, CardType type, int number = -1)
    {
        Color = color;
        Type = type;
        Number = number;
    }

    public bool IsPlayable(Card topCard)
    {
        bool colorMatch = this.Color == topCard.Color;
        bool isWildCard = this.Color == Color.None;
        bool areSameNumber = this.Type == CardType.Number && topCard.Type == CardType.Number && Number == topCard.Number;

        return colorMatch || areSameNumber || isWildCard;
    }

    public void SetColor(Color newColor)
    {
        Color = newColor;
    }

    public override string ToString()
    {
        return Type == CardType.Number ? $"{Color} {Number}" : $"{Color} {Type}";
    }
    public void PrintColored()
    {
        ConsoleColor consoleColor = ConsoleColor.White;

        switch (Color)
        {
            case Color.Red:
                consoleColor = ConsoleColor.Red; break;
            case Color.Blue:
                consoleColor = ConsoleColor.Blue; break;
            case Color.Green:
                consoleColor = ConsoleColor.Green; break;
            case Color.Yellow:
                consoleColor = ConsoleColor.Yellow; break;
            case Color.None:
                consoleColor = ConsoleColor.White; break;
        }

        var oldColor = Console.ForegroundColor;
        Console.ForegroundColor = consoleColor;

        if (Type == CardType.Number)
            Console.Write($"{Color} {Number}");
        else
            Console.Write($"{Color} {Type}");

        Console.ForegroundColor = oldColor; // reset
    }

}