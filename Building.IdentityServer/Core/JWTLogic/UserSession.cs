using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Building.IdentityServer.Core.JWTLogic
{
    public class UserSession : IUserSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserNameSession()
        {
            var userName = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == "username")?.Value;
            return userName;
        }
    }
}
