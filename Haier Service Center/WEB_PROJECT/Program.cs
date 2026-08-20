using ServicePlatform.Data;
using ServicePlatform.Interface;
using ServicePlatform.Models;
using ServicePlatform.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ServicePlatform
{
	public class Program
	{
		public async static Task Main(string[] args)
		{
			var host = CreateHostBuilder(args).Build();
			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var loggerFactory = services.GetRequiredService<ILoggerFactory>();
				try
				{
					var context = services.GetRequiredService<ApplicationDbContext>();
					var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
					var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
					await ContextSeed.SeedRolesAsync(userManager, roleManager);
					await ContextSeed.SeedSuperAdminAsync(userManager, roleManager);
				}
				catch (Exception ex)
				{
					var logger = loggerFactory.CreateLogger<Program>();
					logger.LogError(ex, "An error occurred seeding the DB.");
				}
			}
			host.Run();
		}


		//public static IHostBuilder CreateHostBuilder(string[] args) =>
		//    Host.CreateDefaultBuilder(args)
		//        .ConfigureWebHostDefaults(webBuilder =>
		//        {
		//            webBuilder.UseStartup<Startup>();
		//        });


		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((hostingContext, config) =>
				{
					// Add configuration sources.
					config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
					config.AddEnvironmentVariables();
				})
				.ConfigureServices((context, services) =>
				{
					// Register the configuration class
					services.Configure<AzureStorageConfig>(context.Configuration.GetSection("AzureStorage"));

					// Register the BlobService
					services.AddSingleton<BlobService>();
				})
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
					webBuilder.UseWebRoot("wwwroot");
					webBuilder.UseStartup<Startup>();
				});
	}

}
