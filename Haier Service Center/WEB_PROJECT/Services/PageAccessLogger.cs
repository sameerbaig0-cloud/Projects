using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using ServicePlatform.Data;
using ServicePlatform.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace ServicePlatform.Services
{
    public class PageAccessLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public PageAccessLoggingMiddleware(RequestDelegate next, IConfiguration configuration, HttpClient httpClient)
        {
            _next = next;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task Invoke(HttpContext context, ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            var userEmailClaim = context.User.FindFirst(ClaimTypes.Email)?.Value;
            if (!string.IsNullOrEmpty(userEmailClaim))
            {
                var user = await userManager.FindByEmailAsync(userEmailClaim);
                if (user != null)
                {
                    var userId = user.Id;
                    var pagePath = context.Request.Path;
                    var accessTime = DateTime.UtcNow;
                    var ipAddress = context.Connection.RemoteIpAddress?.ToString();

                    // Get latitude and longitude
                    var (latitude, longitude) = await GetCoordinatesAsync(ipAddress);

                    var accessLog = new PageAccessLog
                    {
                        UserId = userId,
                        PagePath = pagePath,
                        AccessTime = accessTime,
                        Latitude = latitude.ToString(),
                        Longitude = longitude.ToString(),
                        IpAddress = ipAddress
                    };

                    dbContext.PageAccessLogs.Add(accessLog);
                    await dbContext.SaveChangesAsync();
                }
            }

            await _next(context);
        }


        private async Task<(double Latitude, double Longitude)> GetCoordinatesAsync(string ipAddress)
        {
            try
            {
                //var url = $"https://atlas.microsoft.com/ip/geolocation/json?subscription-key={_configuration["AzureMaps:ApiKey"]}&api-version=1.0&ip={ipAddress}";

                //var response = await _httpClient.GetAsync(url);
                //response.EnsureSuccessStatusCode(); // Throw exception for non-success status codes

                //var responseBody = await response.Content.ReadAsStringAsync();
                //var json = JObject.Parse(responseBody);

                //var latitude = (double)json["results"][0]["position"]["lat"];
                //var longitude = (double)json["results"][0]["position"]["lon"];

                var latitude = 0;
                var longitude = 0;

                return (latitude, longitude);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., HTTP request failure, JSON parsing error)
                Console.WriteLine($"Error: {ex.Message}");
                throw; // Rethrow the exception for the caller to handle
            }
        }


    }
}
