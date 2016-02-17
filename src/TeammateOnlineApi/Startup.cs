﻿using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using TeammateOnlineApi.Database;
using TeammateOnlineApi.Database.Repositories;

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
                .AddJsonFile($"config.{env.EnvironmentName}.json",optional: true)
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
            services.AddCors();

            // Configure SQL connection string
            services.AddEntityFramework().AddSqlServer().AddDbContext<TeammateOnlineContext>(options =>
            {
                options.UseSqlServer(Configuration.GetSection("Database:ConnectionString").Value);
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
            app.UseDeveloperExceptionPage();

            // Configure the HTTP request pipeline.
            //app.UseStaticFiles();

            // Add Cors
            app.UseCors(policy =>
            {
                policy.WithOrigins(
                    Configuration.GetSection("Urls:API").Value,
                    Configuration.GetSection("Urls:UI").Value
                    );
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
            });


            // Add authentication (must run before MVC)
            app.UseCookieAuthentication(options =>
            {
                options.AuthenticationScheme = "Cookies";
                options.AutomaticAuthenticate = true;
                options.AutomaticChallenge = false;
            });

            app.UseCookieAuthentication(options =>
            {
                options.AuthenticationScheme = "3rdPartyLogin";
                options.AutomaticAuthenticate = false;
                options.AutomaticChallenge = false;
            });

            // Google Oauth
            app.UseGoogleAuthentication(options => {
                options.AuthenticationScheme = "Google";
                options.SignInScheme = "3rdPartyLogin";
                options.ClientId = Configuration.GetSection("Oauth:Google:ClientId").Value;
                options.ClientSecret = Configuration.GetSection("Oauth:Google:ClientSecret").Value;
            });
            
            // Facebook Oauth
            app.UseFacebookAuthentication(options => {
                options.AuthenticationScheme = "Facebook";
                options.SignInScheme = "3rdPartyLogin";
                options.AppId = Configuration.GetSection("Oauth:Facebook:AppId").Value;
                options.AppSecret = Configuration.GetSection("Oauth:Facebook:AppSecret").Value;
                options.Scope.Add("email");
                options.Scope.Add("user_friends");
            });
            
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
