using HAF.WebServer.GameServer;
using HAF.WebServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
        public int CreateSession([FromBody] SessionCreateModel model)
        {
            var session = store.CreateNewGameSession(model.SessionName, model.Password);
            session.Players.Add(User.GetPlayer(playerStore));
            return session.SessionId;
        }

        [HttpGet("[action]/{sessionId}")]
        public ActionResult<SessionModel> GetSession(int sessionId)
        {
            var session = store.GetSession(sessionId);
            if (session == null)
                return NotFound();
            return new SessionModel(session);
        }

        [HttpPost("[action]")]
        public ActionResult JoinSession([FromBody] int sessionId)
        {
            var session = store.GetSession(sessionId);
            if (session == null)
                return BadRequest();
            session.Players.Add(User.GetPlayer(playerStore));
            return Ok();
        }
    }
}
