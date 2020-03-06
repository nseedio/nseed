using GettingThingsDone.ApplicationCore.Services;
using GettingThingsDone.Contracts.Interface;
using GettingThingsDone.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSeed;
using NSeed.Abstractions;

namespace DotNetCoreSeeds
{
    internal class FirstStartup : SeedBucketStartup
    {
        protected override void ConfigureServices(ServiceCollection services)
        {
            services.AddDbContextPool<GettingThingsDoneDbContext>(options => options.UseSqlServer($@"Server=(localdb)\mssqllocaldb;Database=SeedingWeedingOutAndDestroyingStartup2;Trusted_Connection=True;ConnectRetryCount=0"));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfAsyncRepository<>));
            services.AddScoped<IActionService, ActionService>();
            services.AddScoped<IActionListService, ActionListService>();
            services.AddScoped<IProjectService, ProjectService>();

            services.AddMemoryCache();
        }

        protected override void InitializeSeeding(ServiceProvider serviceProvider, IOutputSink output)
        {
            output.WriteMessage("Ensuring database is created.");

            var dbContext = serviceProvider.GetRequiredService<GettingThingsDoneDbContext>();
            dbContext.Database.EnsureCreated();
        }
    }
}
