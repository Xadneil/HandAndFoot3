using Microsoft.AspNetCore.Mvc.Filters;

namespace HAF.WebServer.GameServer
{
    sealed class CurrentTurnValidator : IActionFilter
    {
        private readonly PlayerStore playerStore;

        public CurrentTurnValidator(PlayerStore playerStore)
        {
            this.playerStore = playerStore;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.GetSession();
            if (session == null)
            {
                context.Result = new Microsoft.AspNetCore.Mvc.BadRequestObjectResult("No Session");
                return;
            }
            if (session.Game.Player != context.HttpContext.User.GetPlayer(playerStore).PlayerIndex)
            {
                context.Result = new Microsoft.AspNetCore.Mvc.BadRequestObjectResult("Not your turn");
                return;
            }
        }
    }
}
