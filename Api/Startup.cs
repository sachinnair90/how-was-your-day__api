using DataAccess;
using DataAccess.Repositories;
using Infrastructure;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using BusinessLogic.Interfaces;
using BusinessLogic;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;

namespace Api
{
    public class Startup
    {
        private const string CONFIGURATION_KEY = "AppSettings";
        private const string DB_NAME = "HWYD";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                var defaultPolicy =
                new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.Filters.Add(new AuthorizeFilter(defaultPolicy));
            });

            services.AddOptions();

            AddDependencies(services);

            services.AddDbContext<AppDBContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString(DB_NAME)));

            var appSettingsSection = Configuration.GetSection(CONFIGURATION_KEY);
            services.Configure<Configuration>(appSettingsSection);

            AddAuthentication(services, appSettingsSection);

            services.AddAutoMapper(typeof(DataMapper));

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "How was your day API", Version = "v1" }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "How was your day API V1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private static void AddDependencies(IServiceCollection services)
        {
            // Business Logic DI
            services.AddScoped<ILoginService, LoginService>();

            // Data Access DI
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

            // Infrastructure DI
            services.AddScoped<IHashHelpers, HashHelpers>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();

            // Framework and others
            services.AddScoped((options) => new JwtSecurityTokenHandler());
        }

        private static void AddAuthentication(IServiceCollection services, IConfigurationSection appSettingsSection)
        {
            // configure jwt authentication
            var security = appSettingsSection.Get<Configuration>().Security;
            var key = Encoding.ASCII.GetBytes(security.JwtSecret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
    }
}
