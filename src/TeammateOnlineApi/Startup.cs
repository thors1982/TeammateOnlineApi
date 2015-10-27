using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Data.Entity;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using TeammateOnlineApi.Database;
using Microsoft.Framework.Configuration;
using Microsoft.Dnx.Runtime;
using AutoMapper;
using TeammateOnlineApi.Models;
using TeammateOnlineApi.ViewModels;

namespace TeammateOnlineApi
{
    public class Startup
    {
        public Microsoft.Framework.Configuration.IConfiguration Configuration { get; set; }

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
            // Uncomment the following line to add Web API services which makes it easier to port Web API 2 controllers.
            // You will also need to add the Microsoft.AspNet.Mvc.WebApiCompatShim package to the 'dependencies' section of project.json.
            // services.AddWebApiConventions();

            services.AddLogging();

            // Configure SQL connection string
            services.AddEntityFramework().AddSqlServer().AddDbContext<TeammateOnlineContext>(options =>
            {
                options.UseSqlServer(Configuration.GetSection("Data:ConnectionString").Value);
            });

            // Add swagger as a service
            /* (Doesn't work in beta8 yet)
            services.AddSwagger();
            */
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.MinimumLevel = LogLevel.Warning;
            //loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            Mapper.Initialize(config =>
            {
                config.CreateMap<GamePlatform, GamePlatformViewModel>().ReverseMap();
            });

            // Add the platform handler to the request pipeline.
            app.UseIISPlatformHandler();

            // Configure the HTTP request pipeline.
            //app.UseStaticFiles();

            // Add MVC to the request pipeline.
            app.UseMvc();
            // Add the following route for porting Web API 2 controllers.
            // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");

            // Seed data and add sample data for testing
            SeedData.Initialize(app.ApplicationServices);
            SampleData.Initialize(app.ApplicationServices);

            // Setup swagger
            /*  (Doesn't work in beta8 yet)
            app.UseSwagger();
            app.UseSwaggerUi("docs/");
            */
        }
    }
}
