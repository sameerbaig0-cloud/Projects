using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServicePlatform.Data;
using ServicePlatform.Models;
using ServicePlatform.Interface;
using ServicePlatform.Services;
using Microsoft.AspNetCore.Http.Features;
//using PageAccessLogger.Middleware;
//using ClickEventAuthorizeAttribute.Middleware;
using System.Net;
using Microsoft.Extensions.Options;


namespace ServicePlatform
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{

			services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Connectionstr")));
			services.AddIdentity<ApplicationUser, IdentityRole>()
					.AddEntityFrameworkStores<ApplicationDbContext>()
					.AddDefaultUI()
			.AddDefaultTokenProviders();


			services.Configure<AzureStorageConfig>(Configuration.GetSection("AzureStorage"));


			services.AddCors(options =>
			{
				options.AddPolicy("EnableCORS", builder =>
				{
					builder.AllowAnyOrigin()
					//.WithOrigins("https://baershoes.blob.core.windows.net/")
					.WithOrigins(Configuration["AzureStorage:AzureCorsOrigins"])

					//builder.WithOrigins("http://192.168.2.11:98", "http://192.168.2.11:97")
					.AllowAnyHeader()
					.AllowAnyMethod();
				});
			});

			//services.Configure<FormOptions>(options =>
			//{
			//	options.MultipartBodyLengthLimit = 1048576; // Set the maximum file size in bytes (1 MB in this example)
			//});

			// Register the custom anti-forgery filter as a global filter
			services.AddScoped<ValidateAntiForgeryHeader>();
			services.AddControllersWithViews(options =>
			{
				options.Filters.AddService<ValidateAntiForgeryHeader>();				
			});

            services.AddControllersWithViews().AddRazorRuntimeCompilation();


            //services.AddControllersWithViews();
            services.AddRazorPages();

			//services.AddMvc().AddRazorPagesOptions(options =>
			//{
			//	options.RootDirectory = "/Views";
			//});

			services.AddScoped<UserManager<ApplicationUser>>();

			// Register HttpClient
			services.AddHttpClient();

			// Register middleware
			//services.AddTransient<PageAccessLoggingMiddleware>();

			//services.AddControllersWithViews();


			services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = $"/Identity/Account/Login";
				options.LogoutPath = $"/Identity/Account/Logout";
				options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
			});



			// Register the blob service globally
			services.AddTransient<AzureStorageConfig>();
			services.AddTransient<BlobService>();

			// Register the email service globally
			services.AddTransient<IEmailService, SmtpEmailService>();

			// Register the article picture service globally
			services.AddTransient<IArticlePictureService, ArticlePictureService>();


			// Other service registrations
			services.AddSingleton<INumberToWordsConverter, NumberToWordsConverter>();
			// Other service registrations


		}


		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		[System.Obsolete]
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)//, ILoggerFactory loggerFactory)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			//app.UseMiddleware<RequestSizeMiddleware>();

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();


			app.UseStatusCodePages(context =>
			{
				var response = context.HttpContext.Response;
				if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
				{
					response.Redirect("/Login");
				}
				else if (response.StatusCode == (int)HttpStatusCode.Forbidden)
				{
					response.Redirect("/AccessDenied");
				}
				return Task.CompletedTask;
			});



			app.UseCors("EnableCORS");
			app.UseAuthentication();
			app.UseAuthorization();


			//// Register the page access logging middleware
			app.UseMiddleware<PageAccessLoggingMiddleware>();

			//// Register the ClickEventAuthorizeMiddleware
			app.UseMiddleware<ClickEventAuthorizeMiddleware>();


			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
				endpoints.MapRazorPages();



				//// Route to the AccessDenied Razor Page
				//endpoints.Map("/AccessDenied", async context =>
				//{
				//	// Check if the request is already redirected to the AccessDenied page
				//	if (!context.Request.Path.Value.Contains("/AccessDenied"))
				//	{
				//		// Redirect to the AccessDenied page
				//		//context.Response.Redirect("/AccessDenied");
				//		context.Response.Redirect($"{context.Request.Scheme}://{context.Request.Host}/AccessDenied");

				//	}
				//	else
				//	{
				//		// If already redirected to AccessDenied, return a 404 response
				//		context.Response.StatusCode = 404;
				//	}
				//});

			});


		}
	}
}
