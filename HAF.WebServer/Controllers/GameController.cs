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
        [HttpGet("[action]")]
        public async Task<ActionResult<SinglePlayerGameState>> InitAsync()
        {
            var session = HttpContext.GetSession();
            if (session == null)
                return BadRequest();
            using (await session.LockAsync())
            {
                if (session.Game == null)
                    session.Game = new HandAndFootGame();
            }

            return new SinglePlayerGameState(session.Game, HttpContext.GetPlayer());
        }

        [HttpPost("[action]")]
        [TypeFilter(typeof(CurrentTurnValidator))]
        public ActionResult<SinglePlayerGameState> Draw()
        {
            var session = HttpContext.GetSession();
            if (session == null)
                return BadRequest();
            if (session.Game.PlayerHasDrawn)
                return BadRequest("Player has already drawn");
            session.Game.DrawTwoCards();
            return new SinglePlayerGameState(session.Game, HttpContext.GetPlayer());
        }
    }
}
