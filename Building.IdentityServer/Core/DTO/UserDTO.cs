namespace Building.IdentityServer.Core.DTO
{
    /// <summary>
    /// Object used to map the information about the user
    /// </summary>
    public class UserDTO
    {
        public string UserName { get; set; }
        public string Id { get; set; }
        public string Token { get; set; }   
    }
}
