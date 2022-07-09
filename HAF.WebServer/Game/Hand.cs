using System.Collections.Generic;

namespace HAF.WebServer.Game
{
    public static class Hand
    {
        public static List<Card> CreateHand(List<Card> DrawPile, int cardsPerHand)
        {
            var hand = new List<Card>();
            for (int i = 0; i < cardsPerHand; i++)
            {
                hand.Add(DrawPile[^1]);
                DrawPile.RemoveAt(DrawPile.Count - 1);
            }
            return hand;
        }
    }
}
