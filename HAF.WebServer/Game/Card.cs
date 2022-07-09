using System;

namespace HAF.WebServer.Game
{
    public class Card
    {
        public Rank Rank { get; private set; }
        public Suit Suit { get; private set; }

        public Card(Rank r, Suit s)
        {
            Rank = r;
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
    }
}
