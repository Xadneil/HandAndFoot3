using System;

namespace HAF.WebServer.Game
{
    public struct Card
    {
        public readonly Rank Rank;
        public readonly Suit Suit;

        public Card(Rank r, Suit s)
        {
            Rank = r;
            Suit = s;
        }

        public Card(string r, Suit s)
        {
            Rank = RankUtils.GetRank(r);
            Suit = s;
        }

        public static Card Joker()
        {
            return new Card(Rank.JOKER, Suit.HEARTS);
        }

        public int Points()
        {
            return Rank switch
            {
                Rank.ACE or Rank.TWO => 20,
                Rank.JOKER => 50,
                Rank.THREE => Suit.IsRed() ? 500 : 0,
                Rank.FOUR or Rank.FIVE or Rank.SIX or Rank.SEVEN or Rank.EIGHT => 5,
                Rank.NINE or Rank.TEN or Rank.JACK or Rank.QUEEN or Rank.KING => 10,
                _ => throw new InvalidOperationException($"The rank {Rank} is not valid."),
            };
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj is not Card other)
                return false;
            return Suit == other.Suit && Rank == other.Rank;
        }

        public override int GetHashCode()
        {
            return 37 * Suit.GetHashCode() + Rank.GetHashCode();
        }

        public static bool operator ==(Card left, Card right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Card left, Card right)
        {
            return !(left == right);
        }
    }
}
