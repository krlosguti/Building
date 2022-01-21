using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Building.PropertyAPI.RemoteService.JWT
{
    public class UserSession : IUserSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetTokenSession()
        {
            var token = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == "Token")?.Value;
            return token;
        }
    }
}
