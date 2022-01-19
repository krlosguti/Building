using Building.IdentityServer.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Building.IdentityServer.Core.Context
{
    public class IdentityData
    {
        public static async Task AddUser(ApplicationDbContext context, UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new User
                {
                    UserName = "marce",
                    Email = "marcenokia@hotmail.com"
                };
                await userManager.CreateAsync(user,"Marce1915*");
            }
        }
    }
}
