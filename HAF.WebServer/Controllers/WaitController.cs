using HAF.WebServer.GameServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HAF.WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaitController : ControllerBase
    {
        private readonly SessionStore sessionStore;

        public WaitController(SessionStore sessionStore)
        {
            this.sessionStore = sessionStore;
        }

        [HttpGet("[action]/{sessionId}")]
        [AllowAnonymous]
        public async Task WaitForGameStart(int sessionId)
        {
            var session = sessionStore.GetSession(sessionId);
            if (session == null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var websocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                var data = new byte[1024];
                // It seems like if we do timeout with a cancellation token, the websocket
                // is closed/aborted without a chance to send a reason. So this only has meaning
                // when it the websocket is closed normally.
                // The client is probably not listening for a close reason anyway if we are not
                // doing normal closure.
                string closeReason = "Start";
                do
                {
                    using var cancellationSource = new CancellationTokenSource(5000);
                    var timeoutToken = cancellationSource.Token;
                    try
                    {
                        await websocket.ReceiveAsync(data, timeoutToken);
                    }
                    catch (OperationCanceledException) { }
                    if (timeoutToken.IsCancellationRequested)
                    {
                        closeReason = "Ping timeout";
                        break;
                    }
                    await websocket.SendAsync(
                        Encoding.UTF8.GetBytes(session.Players.Count.ToString()),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None);
                    if (session.Players.Count >= 4)
                        break;
                    await Task.Delay(1000);
                } while (session.Players.Count <= 4 && websocket.State == WebSocketState.Open);
                if (websocket.State == WebSocketState.Open)
                {
                    await websocket.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        closeReason,
                        CancellationToken.None);
                }
            }
            else
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}
