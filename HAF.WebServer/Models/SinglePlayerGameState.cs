using HAF.WebServer.Game;
using HAF.WebServer.GameServer;
using System.Collections.Generic;
using System.Linq;

namespace HAF.WebServer.Models
{
    public class SinglePlayerGameState
    {
        public CardHolder CardHolder { get; set; }
        public IEnumerable<Book> MyTeamTable { get; set; }
        public IEnumerable<Book> TheirTeamTable { get; set; }
        public int DiscardPileSize { get; set; }
        public int DrawPileSize { get; set; }
        public int Round { get; set; }
        public Card Discard { get; set; }

        public SinglePlayerGameState(HandAndFootGame game, Player player)
        {
            CardHolder = game.Teams[player.TeamIndex].CardHolders[player.PlayerIndex];
            MyTeamTable = game.Teams[player.TeamIndex].Books;
            TheirTeamTable = game.Teams[(player.TeamIndex + 1) % 2].Books;
            DiscardPileSize = game.DiscardPile.Count;
            DrawPileSize = game.DrawPile.Count;
            Round = game.Round;
            Discard = game.DiscardPile.LastOrDefault();
        }
    }
}
