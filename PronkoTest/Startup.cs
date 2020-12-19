using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Services;
using Services.Contracts;

namespace PronkoTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            services.ConfigureCors();

            services.ConfigureSwagger();
            
            services.ConfigureDbContext();

            services.AddAutoMapper(typeof(Startup));
            
            services.ConfigureRepositoryManager();

            services.AddScoped<IUserService, UserService>();
            
            services.ConfigureIdentity();
            services.ConfigureJwt();

        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PronkoTest v1"));
            }

            app.UseSerilogRequestLogging();

            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}