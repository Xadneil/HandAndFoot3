using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HAF.WebServer.Game
{
    public class Book : ICollection<Card>
    {
        public readonly Rank Rank;
        public int CountWild { get; protected set; }
        public int CountNatural { get; protected set; }
        public bool IsDirty
        {
            get
            {
                return CountWild > 0;
            }
        }

        private readonly List<Card> cards;

        public Book(IEnumerable<Card> cards)
        {
            if (cards == null || cards.Count() < 3)
            {
                throw new ArgumentNullException(nameof(cards), "There must be at least three cards in a book.");
            }

            var wilds = cards.Where(c => c.Rank.IsWild());
            var naturals = cards.Where(c => !c.Rank.IsWild());

            if (wilds.Count() > naturals.Count())
            {
                throw new WildNaturalBalanceException();
            }

            if (wilds.Any() && cards.Count() > 7)
            {
                throw new InvalidOperationException("A completed dirty book can only contain 7 cards.");
            }

            if (naturals.Select(c => c.Rank).Distinct().Count() != 1)
            {
                throw new ArgumentOutOfRangeException(nameof(cards), "There must be at only one rank among the natural cards in a book.");
            }

            Rank = naturals.Select(c => c.Rank).First();
            this.cards = cards.ToList();
            CountNatural = naturals.Count();
            CountWild = wilds.Count();
        }

        public int Score()
        {
            var score = cards.Sum(c => c.Points());
            if (IsComplete)
            {
                score += IsDirty ? 300 : 500;
            }
            return score;
        }

        public bool IsComplete
        {
            get
            {
                return cards.Count >= 7;
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
            if (item.Rank.IsWild() && CountWild + 1 == CountNatural)
            {
                throw new WildNaturalBalanceException();
            }

            if (IsComplete && IsDirty)
                throw new InvalidOperationException("A completed dirty book can only contain 7 cards.");

            if (IsComplete && !IsDirty && item.Rank.IsWild())
                throw new InvalidOperationException("You cannot add a wild card to a completed clean book.");

            if (item.Rank.IsWild())
                CountWild++;
            else
                CountNatural++;

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
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return cards.GetEnumerator();
        }
    }
}
