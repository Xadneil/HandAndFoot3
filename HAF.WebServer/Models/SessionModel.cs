using HAF.WebServer.GameServer;
using System;

namespace HAF.WebServer.Models
{
    public class SessionModel
    {
        public int SessionId { get; set; }
        public string SessionName { get; }
        public bool HasPassword { get; }
        public DateTime LastUpdated { get; }
        public int NumberOfPlayers { get; }

        public SessionModel(GameSession session)
        {
            SessionId = session.SessionId;
            SessionName = session.SessionName;
            HasPassword = !string.IsNullOrEmpty(session.Password);
            LastUpdated = session.LastUpdated;
            NumberOfPlayers = session.Players.Count;
        }
    }
}
