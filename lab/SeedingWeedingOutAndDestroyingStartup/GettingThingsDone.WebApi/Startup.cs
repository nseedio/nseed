using GettingThingsDone.ApplicationCore.Services;
using GettingThingsDone.Contracts.Interface;
using GettingThingsDone.Infrastructure.Database;
using GettingThingsDone.WebApi.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.IO.Compression;
using System.Text;

namespace GettingThingsDone.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // var inMemoryDatabaseRoot = new InMemoryDatabaseRoot();
            // services.AddDbContextPool<GettingThingsDoneDbContext>(options => options.UseInMemoryDatabase("GettingThingsDoneDatabase", inMemoryDatabaseRoot));
            services.AddDbContextPool<GettingThingsDoneDbContext>(options => options.UseSqlServer($@"Server=(localdb)\mssqllocaldb;Database=SeedingWeedingOutAndDestroyingStartup2;Trusted_Connection=True;ConnectRetryCount=0"));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfAsyncRepository<>));
            services.AddScoped<IActionService, ActionService>();
            services.AddScoped<IActionListService, ActionListService>();
            services.AddScoped<IProjectService, ProjectService>();

            // Add authentication JWT options settings.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "gettingthingsdone.com",
                        ValidAudience = "gettingthingsdone.com",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecurityKey"]))
                    };
                });

            // Add custom authorization claim based policy.
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    CustomPolicies.OnlyUsersOlderThan,
                    policy => policy
                        .RequireClaim(CustomClaimTypes.DateOfBirth)
                        .AddRequirements(new OlderThanRequirement(50)));
            });

            // Register Older Than authorization handler.
            services.AddSingleton<IAuthorizationHandler, OlderThanAuthorizationHandler>();

            // Add versioning API.
            services.AddApiVersioning(v =>
            {
                v.AssumeDefaultVersionWhenUnspecified = true;
                v.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });

            // Add Response Caching service.
            services.AddResponseCaching();

            // Add InMemory cache.
            services.AddMemoryCache();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            // Turn on default GZIP compression.
            services.AddResponseCompression();
            // Set GZIP options
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal; // Max reduce more CPU work
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
