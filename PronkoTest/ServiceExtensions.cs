using System;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Repository;
using Repository.Contracts;
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

        public static void ConfigureDbContext(this IServiceCollection services) =>
            services.AddDbContext<RepositoryContext>(opts => 
                opts.UseNpgsql(Environment.GetEnvironmentVariable("PostgreSQLConnection")!, 
                    b => b.MigrationsAssembly("PronkoTest")));

        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager, RepositoryManager>();
    }
}