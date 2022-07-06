using HAF.WebServer.Game;
using System;
using System.Collections.Generic;

namespace HAF.WebServer.GameServer
{
    public class GameSession
    {
        public int SessionId { get; }
        public string SessionName { get; }
        public string Password { get; }
        public DateTime LastUpdated { get; set; }
        public HandAndFootGame Game { get; set; }
        public List<Player> Players { get; } = new List<Player>();

        public GameSession(int sessionId, string sessionName, string password)
        {
            SessionId = sessionId;
            SessionName = sessionName;
            Password = password;
            LastUpdated = DateTime.UtcNow;
        }
    }
}
