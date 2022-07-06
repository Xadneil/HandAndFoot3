using System.Collections.Generic;

namespace HAF.WebServer.GameServer
{
    public class PlayerStore
    {
        private readonly Dictionary<int, Player> players = new Dictionary<int, Player>();
        private readonly Dictionary<string, Player> playersByName = new Dictionary<string, Player>();
        private int nextPlayerId = 0;

        public Player CreateNewPlayer(string playerName)
        {
            playerName = playerName.Trim();
            var player = new Player(playerName, nextPlayerId++);
            players.Add(player.PlayerId, player);
            playersByName.Add(playerName, player);
            return player;
        }

        //public void PurgeOldSessions(TimeSpan expiration)
        //{
        //    foreach (var session in GetAllSessions().Where(s => s.LastUpdated + expiration < DateTime.UtcNow).ToList())
        //    {
        //        gameSessions.Remove(session.SessionId);
        //    }
        //}

        public Player GetPlayerById(int playerId)
        {
            return players.GetValueOrDefault(playerId);
        }

        public Player GetPlayerByName(string name)
        {
            return playersByName.GetValueOrDefault(name.Trim());
        }
    }
}
