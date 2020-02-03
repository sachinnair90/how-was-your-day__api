using Api.Infrastructure;
using DataAccess;
using Infrastructure;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            CreateDBIfNotExists(host);

            host.Run();
        }

        private static void CreateDBIfNotExists(IHost host)
        {
            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;

            try
            {
                var unitOfWork = services.GetRequiredService<IUnitOfWork>();
                var hashHelpers = services.GetRequiredService<IHashHelpers>();
                var options = services.GetRequiredService<IOptions<Configuration>>();

                DBInitializer.Initialize(unitOfWork, hashHelpers, options);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred - DB Creation");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}