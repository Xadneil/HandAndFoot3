using System;
using System.Collections.Generic;
using System.Linq;

namespace HAF.WebServer.Game
{
    public class HandAndFootGame
    {
        public readonly Team[] Teams;
        public List<Card> DrawPile, DiscardPile;
        public int Round, Team;
        public int[] TeamPoints;
        private readonly int cardsPerHand;
        private readonly int decks;

        private int player;
        public int Player
        {
            get => player;
            set
            {
                if (value == player)
                    return;
                if (!PlayerHasDiscarded)
                    throw new InvalidOperationException("Cannot change current player if player has not discarded.");
                player = value;
                PlayerHasDrawn = false;
                PlayerHasDiscarded = false;
            }
        }
        public bool PlayerHasDrawn;
        public bool PlayerHasDiscarded;

        public HandAndFootGame(int teams = 2, int playersPerTeam = 2, int cardsPerHand = 11, int decks = 5)
        {
            this.cardsPerHand = cardsPerHand;
            this.decks = decks;
            Teams = new Team[teams];
            TeamPoints = new int[teams];
            for (int i = 0; i < teams; i++)
            {
                Teams[i] = new Team(playersPerTeam);
                TeamPoints[i] = 0;
            }

            DiscardPile = new List<Card>();
            DrawPile = new List<Card>(54 * decks);
            for (int i = 0; i < decks; i++)
            {
                foreach (Suit suit in Enum.GetValues(typeof(Suit)))
                {
                    foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                    {
                        DrawPile.Add(new Card(rank, suit));
                    }
                }
                DrawPile.Add(Card.Joker());
                DrawPile.Add(Card.Joker());
            }

            Shuffle(DrawPile);

            Round = 1;

            foreach (CardHolder player in Teams.SelectMany(t => t.CardHolders))
            {
                player.DealCards(Hand.CreateHand(DrawPile, cardsPerHand), Hand.CreateHand(DrawPile, cardsPerHand));
            }
        }

        // shuffle the pile with the Fisher-Yates shuffle
        private static void Shuffle(List<Card> DrawPile)
        {
            var rng = new Random();
            int n = DrawPile.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = DrawPile[k];
                DrawPile[k] = DrawPile[n];
                DrawPile[n] = value;
            }
        }

        private void Reset()
        {
            DiscardPile = new List<Card>();
            DrawPile = new List<Card>(54 * decks);
            for (int i = 0; i < decks; i++)
            {
                foreach (Suit suit in Enum.GetValues(typeof(Suit)))
                {
                    foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                    {
                        DrawPile.Add(new Card(rank, suit));
                    }
                }
                DrawPile.Add(Card.Joker());
                DrawPile.Add(Card.Joker());
            }

            Shuffle(DrawPile);

            foreach (CardHolder player in Teams.SelectMany(t => t.CardHolders))
            {
                player.DealCards(Hand.CreateHand(DrawPile, cardsPerHand), Hand.CreateHand(DrawPile, cardsPerHand));
            }

            foreach (Team team in Teams)
            {
                team.Books.Clear();
            }
        }

        public static void PlayOnBook(CardHolder player, Card card, Book book)
        {
            if (!player.CurrentHand.Remove(card))
                throw new InvalidOperationException($"The player does not have the reported card ({card.Rank + (card.Rank == Rank.JOKER ? "" : (" of " + card.Suit))}) in their hand.");
            book.Add(card);
        }

        public static void PlayNewBook(Team team, CardHolder player, Book book)
        {
            foreach (Card card in book)
            {
                if (!player.CurrentHand.Remove(card))
                    throw new InvalidOperationException($"The player does not have the reported card ({card.Rank + (card.Rank == Rank.JOKER ? "" : (" of " + card.Suit))}) in their hand.");
            }
            team.Books.Add(book);
        }

        public Card[] DrawTwoCards()
        {
            if (PlayerHasDrawn)
                throw new InvalidOperationException("Player has already drawn.");
            Card[] ret = new Card[2];
            for (int i = 0; i < 2; i++)
            {
                if (!DrawPile.Any())
                {
                    if (DiscardPile.Any())
                    {
                        DrawPile = DiscardPile;
                        DiscardPile = new List<Card>();
                        Shuffle(DrawPile);
                    }
                    else
                    {
                        throw new InvalidOperationException("There are not enough cards to draw.");
                    }
                }
                var card = DrawPile[^1];
                DrawPile.RemoveAt(DrawPile.Count - 1);
                Teams[Team].CardHolders[player].CurrentHand.Add(card);
                ret[i] = card;
            }
            PlayerHasDrawn = true;
            return ret;
        }

        public Card[] DrawSevenCardsFromDiscardAndPlayNewBook(Team team, CardHolder player, Card card1, Card card2)
        {
            if (DiscardPile.Count < 7)
                throw new InvalidOperationException("There are not enough cards in the discard pile. It must have 7 cards.");

            if (!player.CurrentHand.Remove(card1))
                throw new InvalidOperationException($"The player does not have the reported card ({card1.Rank + (card1.Rank == Rank.JOKER ? "" : (" of " + card1.Suit))}) in their hand.");
            if (!player.CurrentHand.Remove(card2))
                throw new InvalidOperationException($"The player does not have the reported card ({card2.Rank + (card2.Rank == Rank.JOKER ? "" : (" of " + card2.Suit))}) in their hand.");

            Card[] ret = new Card[6];
            Card topDiscard = DiscardPile[^1];

            for (int i = 0; i < 7; i++)
            {
                var card = DiscardPile[^1];
                DiscardPile.RemoveAt(DiscardPile.Count - 1);
                if (i > 0)
                {
                    player.CurrentHand.Add(card);
                    ret[i - 1] = card;
                }
            }

            team.Books.Add(new Book(new Card[] { card1, card2, topDiscard }));
            return ret;
        }

        public void Discard(Team team, CardHolder player, Card card)
        {
            if (player.CurrentHand.Count == 1 && player.IsInFoot)
            {
                if (team.Books.Count(b => b.IsDirty && b.IsComplete) < 2)
                    throw new InvalidOperationException("The player's team does not have enough complete dirty piles to go out.");
                if (team.Books.Count(b => !b.IsDirty && b.IsComplete) < 2)
                    throw new InvalidOperationException("The player's team does not have enough complete clean piles to go out.");

                // The player is ending a round. Total and accumulate points, and reset teams for another round if applicable.
                foreach (var (team2, index) in Teams.Select((x, i) => (Team: x, Index: i)))
                {
                    TeamPoints[index] += team2.Books.Sum(b => b.Score());
                    TeamPoints[index] -= team2.CardHolders.Where(p => p.Hand != null).SelectMany(p => p.Hand).Sum(c => c.Points());
                    TeamPoints[index] -= team2.CardHolders.Where(p => p.Foot != null).SelectMany(p => p.Foot).Sum(c => c.Points());
                }

                switch (Round)
                {
                    case 1:
                    case 2:
                    case 3:
                        Round++;
                        Reset();
                        break;
                }
            }

            if (!player.CurrentHand.Remove(card))
                throw new InvalidOperationException("The player does not have the reported card in their hand.");
            DiscardPile.Add(card);
        }
    }
}
