using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Server.Db;
using Server.Db.Repository;
using Server.Db.Repository.Interface;
using Server.Models;
using Server.Services;
using Server.Services.Interfaces;


namespace Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContextFactory<ApplicationContext>(options => options.UseNpgsql(connection),
                ServiceLifetime.Transient);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => { options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/auth"); });

            services.AddControllers();
            services.AddTransient<ILogicalElementsService, LogicalElementsService>();
            services.AddTransient<IAuthService, AuthService>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IElementRepository, ElementRepository>();
            services.AddTransient<IConnectionRepository, ConnectionRepository>();

            services.Configure<TokenOptions>(Configuration.GetSection(TokenOptions.SectionName));

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "JwtBearer";
                    options.DefaultChallengeScheme = "JwtBearer";
                })
                .AddJwtBearer("JwtBearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration
                            .GetSection(TokenOptions.SectionName).GetSection("SecretKey").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false
                    };
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}