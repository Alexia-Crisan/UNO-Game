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

    public override string ToString()
    {
        return Type == CardType.Number ? $"{Color} {Number}" : $"{Color} {Type}";
    }
}