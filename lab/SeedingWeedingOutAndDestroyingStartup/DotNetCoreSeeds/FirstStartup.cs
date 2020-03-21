using GettingThingsDone.ApplicationCore.Services;
using GettingThingsDone.Contracts.Interface;
using GettingThingsDone.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSeed;
using NSeed.Abstractions;
using System;

namespace DotNetCoreSeeds
{
    public class FirstStartup : SeedBucketStartup
    {
        private readonly IOutputSink output;

        public FirstStartup(IOutputSink output) // TODO: Validation: can only be parameterless or have IOutputSink as parameter.
        {
            this.output = output;
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            output.WriteVerboseMessage($"Executing {nameof(FirstStartup)}.{nameof(ConfigureServices)}");

            services.AddDbContextPool<GettingThingsDoneDbContext>(options => options.UseSqlServer($@"Server=(localdb)\mssqllocaldb;Database=SeedingWeedingOutAndDestroyingStartup2;Trusted_Connection=True;ConnectRetryCount=0"));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfAsyncRepository<>));
            services.AddScoped<IActionService, ActionService>();
            services.AddScoped<IActionListService, ActionListService>();
            services.AddScoped<IProjectService, ProjectService>();

            services.AddMemoryCache();

            services.AddSingleton<ISomeSingletonService, SomeSingletonService>();
        }

        protected override void InitializeSeeding(IServiceProvider serviceProvider)
        {
            output.WriteVerboseMessage($"Executing {nameof(FirstStartup)}.{nameof(InitializeSeeding)}");

            output.WriteMessage("Ensuring database is created.");

            var dbContext = serviceProvider.GetRequiredService<GettingThingsDoneDbContext>();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }
    }
}
