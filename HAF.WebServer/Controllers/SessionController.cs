using HAF.WebServer.GameServer;
using HAF.WebServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HAF.WebServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : ControllerBase
    {
        private readonly SessionStore store;
        private readonly PlayerStore playerStore;

        public SessionController(SessionStore store, PlayerStore playerStore)
        {
            this.store = store;
            this.playerStore = playerStore;
        }

        [HttpGet("[action]")]
        public IEnumerable<SessionModel> JoinableSessions()
        {
            return store.GetAllSessions().Where(s => s.Game == null).OrderBy(s => s.SessionName).Select(s => new SessionModel(s));
        }

        [HttpPost("[action]")]
        public async Task<SessionResponseModel> CreateSessionAsync([FromBody] SessionCreateModel model)
        {
            var session = store.CreateNewGameSession(model.SessionName, model.Password);
            using (await session.LockAsync())
            {
                session.AddPlayer(User.GetPlayer(playerStore));
            }
            return new SessionResponseModel(session.SessionId, "Wait");
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<SessionResponseModel>> JoinSessionAsync([FromBody] int sessionId)
        {
            var session = store.GetSession(sessionId);
            if (session == null)
                return BadRequest();
            using (await session.LockAsync())
            {
                if (session.Players.Count >= 4)
                    return BadRequest();
                session.AddPlayer(User.GetPlayer(playerStore));
                return new SessionResponseModel(sessionId, session.Players.Count == 4 ? "Start" : "Wait");
            }
        }

        [HttpGet("[action]/{sessionId}")]
        public ActionResult<SessionModel> GetSession(int sessionId)
        {
            var session = store.GetSession(sessionId);
            if (session == null)
                return NotFound();
            return new SessionModel(session);
        }
    }
}
