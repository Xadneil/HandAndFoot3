using Microsoft.AspNetCore.Mvc.Filters;

namespace HAF.WebServer.GameServer
{
    public class GameSessionFilter : IResourceFilter
    {
        private readonly SessionStore sessionStore;

        public GameSessionFilter(SessionStore sessionStore)
        {
            this.sessionStore = sessionStore;
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("X-SessionId", out var values))
                return;
            if (string.IsNullOrWhiteSpace(values[0]) ||
                !int.TryParse(values[0], out int sessionId))
            {
                context.Result = new Microsoft.AspNetCore.Mvc.BadRequestObjectResult("Bad X-SessionId header");
                return;
            }
            var session = sessionStore.GetSession(sessionId);
            if (session == null)
            {
                context.Result = new Microsoft.AspNetCore.Mvc.BadRequestObjectResult("Bad SessionId");
                return;
            }
            context.HttpContext.Items["GameSession"] = session;
        }
    }
}
