using System;
using System.Collections;
using System.Collections.Generic;

namespace HAF.WebServer.Game
{
    public class Hand : ICollection<Card>
    {
        private readonly List<Card> cards = new();

        public Hand(List<Card> DrawPile, int cardsPerHand)
        {
            for (int i = 0; i < cardsPerHand; i++)
            {
                Add(DrawPile[^1]);
                DrawPile.RemoveAt(DrawPile.Count - 1);
            }
        }

        public int Count
        {
            get
            {
                return cards.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add(Card item)
        {
            cards.Add(item);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(Card item)
        {
            return cards.Contains(item);
        }

        public void CopyTo(Card[] array, int arrayIndex)
        {
            cards.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Card> GetEnumerator()
        {
            return cards.GetEnumerator();
        }

        public bool Remove(Card item)
        {
            return cards.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return cards.GetEnumerator();
        }
    }
}
