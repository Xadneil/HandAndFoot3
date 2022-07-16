using HAF.WebServer.GameServer;
using Microsoft.AspNetCore.Http;

namespace HAF.WebServer
{
    public static class HttpContextExtensions
    {
        public static GameSession GetSession(this HttpContext httpContext)
        {
            if (!httpContext.Items.TryGetValue("GameSession", out object session))
                return null;
            return (GameSession)session;
        }

        public static Player GetPlayer(this HttpContext httpContext)
        {
            if (!httpContext.Items.TryGetValue("Player", out object player))
                return null;
            return (Player)player;
        }
    }
}
