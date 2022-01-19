using Building.IdentityServer.Core.Context;
using Building.IdentityServer.Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Building.IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostServer = CreateHostBuilder(args).Build();
            using (var context = hostServer.Services.CreateScope())
            {
                var services = context.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var contextEF = services.GetRequiredService<ApplicationDbContext>();
                    IdentityData.AddUser(contextEF, userManager).Wait();
                }
                catch (Exception ex)
                {
                    var loggin = services.GetRequiredService<ILogger<Program>>();
                    loggin.LogError(ex, "Cann't insert new userManager");
                }
            };

            hostServer.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
