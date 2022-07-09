using HAF.WebServer.Game;
using System;
using System.Collections.Generic;
using System.Threading;

namespace HAF.WebServer.GameServer
{
    public class GameSession
    {
        public SemaphoreSlim Semaphore { get; } = new SemaphoreSlim(1);

        public int SessionId { get; }
        public string SessionName { get; }
        public string Password { get; }
        public DateTime LastUpdated { get; set; }
        public HandAndFootGame Game { get; set; }
        public IReadOnlyList<Player> Players { get => players; }

        private readonly List<Player> players = new();

        public GameSession(int sessionId, string sessionName, string password)
        {
            SessionId = sessionId;
            SessionName = sessionName;
            Password = password;
            LastUpdated = DateTime.UtcNow;
        }

        public void AddPlayer(Player p)
        {
            if (Semaphore.CurrentCount != 0)
                throw new SynchronizationLockException($"{nameof(Semaphore)} must be locked to call AddPlayer.");
            if (players.Contains(p))
                return;
            players.Add(p);
            p.TeamIndex = (players.Count - 1) % 2;
            p.PlayerIndex = (players.Count - 1) / 2;
            p.SessionId = SessionId;
        }
    }
}
