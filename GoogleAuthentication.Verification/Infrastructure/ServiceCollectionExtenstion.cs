using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace GoogleAuthentication.Verification.Infrastructure
{
    public static class ServiceCollectionExtenstion
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Book API",
                    Version = "v1",
                    Description = "This is a test service"
                });
            });

            return services;
        }
    }
}