using System;
using System.Security.Cryptography;
using System.Text;

namespace HAF.WebServer.GameServer
{
    public class Player
    {
        private static readonly char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        private const int secretSize = 48;

        public string Name { get; private set; }
        public int PlayerId { get; private set; }
        public int SessionId { get; set; }
        public int TeamIndex { get; set; }
        public int PlayerIndex { get; set; }
        public string Secret { get; private set; }

        public Player(string name, int playerId)
        {
            Name = name;
            byte[] data = new byte[4 * secretSize];
            using (var crypto = RandomNumberGenerator.Create())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(secretSize);
            for (int i = 0; i < secretSize; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }
            Secret = result.ToString();
            PlayerId = playerId;
        }
    }
}
