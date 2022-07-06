using HAF.WebServer.GameServer;
using System.Security.Claims;

namespace HAF.WebServer
{
    public static class ClaimsPrincipalExtensions
    {
        public static Player GetPlayer(this ClaimsPrincipal principal, PlayerStore playerStore)
        {
            return playerStore.GetPlayerById(int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)));
        }
    }
}
