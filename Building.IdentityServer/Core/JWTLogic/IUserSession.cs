namespace Building.IdentityServer.Core.JWTLogic
{
    /// <summary>
    /// interface to create GetUserNameSession method
    /// </summary>
    public interface IUserSession
    {
        string GetUserNameSession();
    }
}
