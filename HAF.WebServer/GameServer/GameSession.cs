using HAF.WebServer.Game;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HAF.WebServer.GameServer
{
    public class GameSession
    {
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);

        public int SessionId { get; }
        public string SessionName { get; }
        public string Password { get; }
        public DateTime LastUpdated { get; set; }
        public HandAndFootGame Game { get; set; }
        public IReadOnlyList<Player> Players { get => players; }

        private readonly List<Player> players = new();

        public async Task<IDisposable> LockAsync()
        {
            await semaphore.WaitAsync();
            return new SemaphoreReleaser(semaphore);
        }

        public GameSession(int sessionId, string sessionName, string password)
        {
            SessionId = sessionId;
            SessionName = sessionName;
            Password = password;
            LastUpdated = DateTime.UtcNow;
        }

        public void AddPlayer(Player p)
        {
            if (p == null)
                throw new ArgumentNullException(nameof(p));
            if (semaphore.CurrentCount != 0)
                throw new SynchronizationLockException($"{nameof(semaphore)} must be locked to call AddPlayer.");
            if (players.Contains(p))
                return;
            players.Add(p);
            p.TeamIndex = (players.Count - 1) % 2;
            p.PlayerIndex = (players.Count - 1) / 2;
            p.SessionId = SessionId;
        }

        private sealed class SemaphoreReleaser : IDisposable
        {
            private readonly SemaphoreSlim semaphore;
            private bool isDisposed = false;

            public SemaphoreReleaser(SemaphoreSlim semaphore)
            {
                this.semaphore = semaphore;
            }

            public void Dispose()
            {
                if (!isDisposed)
                {
                    isDisposed = true;
                    semaphore.Release();
                }
            }
        }
    }
}
