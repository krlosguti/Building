using Building.IdentityServer.Core.Entities;

namespace Building.IdentityServer.Core.JWTLogic
{
    /// <summary>
    /// Class with a CreateToken method, the objetctive is generates a token using username, email and datetime.now
    /// </summary>
    public interface IJwtGenerator
    {
        string CreateToken(User user);
    }
}
