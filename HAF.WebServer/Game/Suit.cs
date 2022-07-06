namespace HAF.WebServer.Game
{
    public enum Suit
    {
        HEARTS,
        DIAMONDS,
        CLUBS,
        SPADES
    }

    public static class SuitUtils
    {
        public static bool IsRed(this Suit s)
        {
            return s == Suit.HEARTS || s == Suit.DIAMONDS;
        }
    }
}
