using HAF.WebServer.Game;
using HAF.WebServer.GameServer;
using HAF.WebServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HAF.WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly PlayerStore playerStore;
        private readonly SessionStore sessionStore;

        public GameController(PlayerStore playerStore, SessionStore sessionStore)
        {
            this.playerStore = playerStore;
            this.sessionStore = sessionStore;
        }

        [HttpGet("[action]/{sessionId}")]
        public async Task<ActionResult<SinglePlayerGameState>> InitAsync(int sessionId)
        {
            var session = sessionStore.GetSession(sessionId);
            if (session == null)
                return BadRequest();
            try
            {
                await session.Semaphore.WaitAsync();
                if (session.Game == null)
                    session.Game = new HandAndFootGame();
            }
            finally { session.Semaphore.Release(); }
            var player = User.GetPlayer(playerStore);

            return new SinglePlayerGameState(session.Game, player);
        }
    }
}
