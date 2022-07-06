using System;
using System.Collections.Generic;
using System.Linq;

namespace HAF.WebServer.GameServer
{
    public class SessionStore
    {
        private readonly Dictionary<int, GameSession> gameSessions = new Dictionary<int, GameSession>();
        private int nextSessionId = 0;

        public IReadOnlyCollection<GameSession> GetAllSessions()
        {
            return gameSessions.Values;
        }

        public GameSession CreateNewGameSession(string sessionName, string password)
        {
            var gameSession = new GameSession(nextSessionId++, sessionName, password);
            gameSessions.Add(gameSession.SessionId, gameSession);
            return gameSession;
        }

        public void PurgeOldSessions(TimeSpan expiration)
        {
            foreach (var session in GetAllSessions().Where(s => s.LastUpdated + expiration < DateTime.UtcNow).ToList())
            {
                gameSessions.Remove(session.SessionId);
            }
        }

        public GameSession GetSession(int sessionId)
        {
            return gameSessions.GetValueOrDefault(sessionId);
        }
    }
}
