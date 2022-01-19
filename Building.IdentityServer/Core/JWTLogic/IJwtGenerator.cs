using Building.IdentityServer.Core.Entities;

namespace Building.IdentityServer.Core.JWTLogic
{
    public interface IJwtGenerator
    {
        string CreateToken(User user);
    }
}
