
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Faucet.WebApi.Infastructure;
using Faucet.WebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;

namespace Faucet.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddCustomDbContext(Configuration)
                .AddSwagger(Configuration);

            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IBlockchainService, BlockchainService>();

            services.AddTransient<IFaucetService, FaucetService>();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });


            var containerBuilder = new ContainerBuilder();

            containerBuilder.Populate(services);

            return new AutofacServiceProvider(containerBuilder.Build());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var pathBase = Configuration["PATH_BASE"];

            if (!string.IsNullOrEmpty(pathBase))
            {
                loggerFactory.CreateLogger<Startup>().LogDebug("USING PATH BASE '{pathBase}'", pathBase);
                app.UsePathBase(pathBase);
            }
            app.UseStaticFiles();
            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{(!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty)}/swagger/v1/swagger.json", "Faucet.API V1");
                    c.OAuthClientId("faucetswaggerui");
                    c.OAuthAppName("Faucet Swagger UI");
                });


            app.UseRouting();
            app.UseCors("CorsPolicy");
            ConfigureAuth(app);
            app.UseEndpoints(endpoins =>
            {
                endpoins.MapDefaultControllerRoute();
                endpoins.MapControllers();
            });


        }

        protected virtual void ConfigureAuth(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }

    public static class StartupExtensions
    {


        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<UserTransactionContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionString"], sqlServerOptionsAction =>
                {
                    sqlServerOptionsAction.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                    sqlServerOptionsAction.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                });
            });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Faucet HTTP Api",
                    Version = "v1",
                });

            });



            return services;
        }
    }
}
