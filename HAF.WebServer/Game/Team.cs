using System.Collections.Generic;

namespace HAF.WebServer.Game
{
    public class Team
    {
        public readonly CardHolder[] CardHolders;
        public readonly List<Book> Books;

        public Team(int playersPerTeam)
        {
            CardHolders = new CardHolder[playersPerTeam];
            for (int i = 0; i < playersPerTeam; i++)
            {
                CardHolders[i] = new CardHolder();
            }
            Books = new List<Book>();
        }
    }
}
