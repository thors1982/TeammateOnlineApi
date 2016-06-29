using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
//using Swashbuckle.SwaggerGen.Generator;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using TeammateOnlineApi.Database;
using TeammateOnlineApi.Database.Repositories;

namespace TeammateOnlineApi
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Cors support to the service
            services.AddCors();

            // Add framework services.
            services.AddMvc();

            // Add repositories
            services.AddScoped<IGamePlatformRepository, GamePlatformRepository>();
            services.AddScoped<IGameAccountRepository, GameAccountRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            services.AddScoped<IFriendRepository, FriendRepository>();

            // Configure SQL connection string
            services.AddDbContext<TeammateOnlineContext>(
                options => options.UseSqlServer(Configuration.GetSection("Database:ConnectionString").Value)
                );

            // Add swagger as a service
            /*services.AddSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = Configuration.GetSection("AppSettings:SiteTitle").Value,
                    Description = "",
                });
            });*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            // Add Cors
            app.UseCors(builder =>
            {
                builder.WithOrigins(Configuration.GetSection("Urls:UI").Value)
                .AllowAnyHeader()
                .AllowAnyMethod();
                //.AllowCredentials();
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap = new Dictionary<string, string>();

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                Authority = Configuration.GetSection("Urls:Identity").Value,
                RequireHttpsMetadata = false,

                Audience = Configuration.GetSection("Urls:Identity").Value + "/resources",
                AutomaticAuthenticate = true
            });

            // Add MVC to the request pipeline.
            app.UseMvc();

            // Seed data for application
            SeedData.Initialize(app.ApplicationServices);
            // Add sample data for testing
            if (env.EnvironmentName == "Development")
            {
                SampleData.Initialize(app.ApplicationServices);
            }

            // Setup swagger
            //app.UseSwaggerGen();
            //app.UseSwaggerUi("docs");
        }
    }
}
