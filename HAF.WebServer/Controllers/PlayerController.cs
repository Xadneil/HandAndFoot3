using HAF.WebServer.GameServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HAF.WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly PlayerStore playerStore;

        public PlayerController(PlayerStore playerStore)
        {
            this.playerStore = playerStore;
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public ActionResult<Player> Login([FromBody] string playerName)
        {
            if (string.IsNullOrWhiteSpace(playerName))
                return BadRequest();
            Player player = playerStore.GetPlayerByName(playerName);
            if (player != null)
                return player;
            return playerStore.CreateNewPlayer(playerName);
        }
    }
}
