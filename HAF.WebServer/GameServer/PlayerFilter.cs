using Microsoft.AspNetCore.Mvc.Filters;

namespace HAF.WebServer.GameServer
{
    public class PlayerFilter : IResourceFilter
    {
        private readonly PlayerStore playerStore;

        public PlayerFilter(PlayerStore playerStore)
        {
            this.playerStore = playerStore;
        }


        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
                return;
            context.HttpContext.Items["Player"] = context.HttpContext.User.GetPlayer(playerStore);
        }
    }
}
