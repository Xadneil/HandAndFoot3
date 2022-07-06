using HAF.WebServer.GameServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HAF.WebServer
{
    public class PlayerIdAndSecretAuthenticationHandler : IAuthenticationHandler
    {
        public const string authenticationScheme = "PlayerIdAndSecretHeaders";
        private readonly PlayerStore playerStore;
        private IHeaderDictionary headers;

        public PlayerIdAndSecretAuthenticationHandler(PlayerStore playerStore)
        {
            this.playerStore = playerStore;
        }

        public Task<AuthenticateResult> AuthenticateAsync()
        {
            if (this.headers == null)
                return Task.FromResult(AuthenticateResult.Fail("No Headers"));

            string getHeader(string name)
            {
                if (!headers.ContainsKey(name))
                    return null;
                return headers[name][0];
            }

            var playerIdString = getHeader("X-PlayerId");
            if (string.IsNullOrEmpty(playerIdString))
                return Task.FromResult(AuthenticateResult.Fail("Missing X-PlayerId Header"));
            var secret = getHeader("X-Secret");
            if (string.IsNullOrEmpty(secret))
                return Task.FromResult(AuthenticateResult.Fail("Missing X-Secret Header"));

            if (!int.TryParse(playerIdString, out int playerId))
                return Task.FromResult(AuthenticateResult.Fail("PlayerId is not a number"));

            var player = playerStore.GetPlayerById(playerId);
            if (player == null)
                return Task.FromResult(AuthenticateResult.Fail("No player with that ID"));
            if (player.Secret != secret)
                return Task.FromResult(AuthenticateResult.Fail("Secret does not match"));

            var identity = new ClaimsPrincipal(new[]
            {
                new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, player.Name),
                    new Claim(ClaimTypes.NameIdentifier, playerIdString),
                }, authenticationScheme)
            });

            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(identity, authenticationScheme)));
        }

        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            throw new System.NotImplementedException();
        }

        public Task ForbidAsync(AuthenticationProperties properties)
        {
            throw new System.NotImplementedException();
        }

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            headers = context.Request.Headers;
            return Task.CompletedTask;
        }
    }
}
