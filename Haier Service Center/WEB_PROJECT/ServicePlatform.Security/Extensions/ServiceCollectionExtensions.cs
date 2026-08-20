using Microsoft.Extensions.DependencyInjection;
using ServicePlatform.Security.Encryption;
using ServicePlatform.Security.ModelBinders;

namespace ServicePlatform.Security.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMvcUrlEncryption(
            this IServiceCollection services)
        {
            services.AddDataProtection();

            services.AddSingleton<IUrlEncryptionService, UrlEncryptionService>();

            services.Configure<Microsoft.AspNetCore.Mvc.MvcOptions>(options =>
            {
                options.ModelBinderProviders.Insert(
                    0,
                    new EncryptedIdModelBinderProvider());
            });

            return services;
        }
    }
}

