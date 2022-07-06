using System;

namespace HAF.WebServer.Game
{
    public enum Rank
    {
        ACE = 1,
        TWO,
        THREE,
        FOUR,
        FIVE,
        SIX,
        SEVEN,
        EIGHT,
        NINE,
        TEN,
        JACK,
        QUEEN,
        KING,
        JOKER
    }

    public static class RankUtils
    {
        public static bool IsWild(this Rank r)
        {
            return r == Rank.TWO || r == Rank.JOKER;
        }

        public static Rank GetRank(string abbreviation)
        {
            return abbreviation switch
            {
                "A" => Rank.ACE,
                "2" => Rank.TWO,
                "3" => Rank.THREE,
                "4" => Rank.FOUR,
                "5" => Rank.FIVE,
                "6" => Rank.SIX,
                "7" => Rank.SEVEN,
                "8" => Rank.EIGHT,
                "9" => Rank.NINE,
                "10" => Rank.TEN,
                "J" => Rank.JACK,
                "Q" => Rank.QUEEN,
                "K" => Rank.KING,
                "*J*" => Rank.JOKER,
                _ => throw new ArgumentOutOfRangeException(nameof(abbreviation)),
            };
        }
    }
}
