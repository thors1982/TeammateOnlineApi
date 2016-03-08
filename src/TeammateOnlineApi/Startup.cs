using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.SwaggerGen;
using TeammateOnlineApi.Database;
using TeammateOnlineApi.Database.Repositories;
using TeammateOnlineApi.Configs;

namespace TeammateOnlineApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddJsonFile($"config.{env.EnvironmentName}.json",optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.Configure<UrlConfig>(Configuration.GetSection("Urls"));

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
            services.ConfigureSwaggerDocument(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "TeammateOnline API",
                    Description = "",
                    TermsOfService = "None"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.MinimumLevel = LogLevel.Warning;
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            // Add the platform handler to the request pipeline.
            app.UseIISPlatformHandler();
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
                app.UseRuntimeInfoPage("/runtimeinfo");
            }

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

            // Seed data for application
            SeedData.Initialize(app.ApplicationServices);
            // Add sample data for testing
            if (env.EnvironmentName == "Development")
            {
                SampleData.Initialize(app.ApplicationServices);
            }

            // Setup swagger
            app.UseSwaggerGen();
            app.UseSwaggerUi("docs");
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
