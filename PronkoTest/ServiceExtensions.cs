using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;

namespace PronkoTest
{
    public static class ServiceExtensions
    {
        public static void ConfigureSwagger(this IServiceCollection services) =>
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "PronkoTest", Version = "v1"});
            });
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options => 
                options.AddPolicy("CorsPolicy",builder =>
                {
                    builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins("https://localhost:4200", "http://localhost:4200");
                }));
    }
}