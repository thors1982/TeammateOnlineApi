using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeammateOnlineApi.Database;
using TeammateOnlineApi.Database.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;

namespace TeammateOnlineApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public static void Main(string[] args) => WebApplication.Run<Startup>(args);

        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by a runtime.
        // Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // Add repositories
            services.AddScoped<IGamePlatformRepository, GamePlatformRepository>();
            services.AddScoped<IGameAccountRepository, GameAccountRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            services.AddScoped<IFriendRepository, FriendRepository>();

            // Add Cors support to the service
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                                                                        .AllowAnyMethod()
                                                                         .AllowAnyHeader()));

            // Configure SQL connection string
            services.AddEntityFramework().AddSqlServer().AddDbContext<TeammateOnlineContext>(options =>
            {
                options.UseSqlServer(Configuration.GetSection("Data:ConnectionString").Value);
            });

            // Add swagger as a service
            services.AddSwaggerGen();
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.MinimumLevel = LogLevel.Warning;
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            // Add the platform handler to the request pipeline.
            app.UseIISPlatformHandler();

            app.UseCors("AllowAll");

            // Configure the HTTP request pipeline.
            //app.UseStaticFiles();

            // Add MVC to the request pipeline.
            app.UseMvc();

            // Seed data and add sample data for testing
            SeedData.Initialize(app.ApplicationServices);
            SampleData.Initialize(app.ApplicationServices);

            // Setup swagger
            app.UseSwaggerGen();
            app.UseSwaggerUi("docs");
        }
    }
}
