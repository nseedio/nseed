using DotNetCoreSeeds;
using GettingThingsDone.ApplicationCore.Services;
using GettingThingsDone.Contracts.Interface;
using GettingThingsDone.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NSeed;
using NSeed.Abstractions;
using System;

namespace GettingThingsDone.ApplicationCore.Tests.Unit
{
    internal class SampleStartupForUnitTests : SeedBucketStartup
    {
        public static readonly InMemoryDatabaseRoot SharedInMemoryDatabaseRoot = new InMemoryDatabaseRoot();
        public static readonly string SharedInMemoryDatabaseName = "SharedInMemoryDatabase";

        private readonly IOutputSink output;

        public SampleStartupForUnitTests(IOutputSink output) // TODO: Validation: can only be parameterless or have IOutputSink as parameter.
        {
            this.output = output;
        }

        public string? DatabaseName { get; set; }

        public InMemoryDatabaseRoot? DatabaseRoot { get; set; }

        protected override void ConfigureServices(IServiceCollection services)
        {
            output.WriteVerboseMessage($"Executing {nameof(SampleStartupForUnitTests)}.{nameof(ConfigureServices)}");

            var databaseName = DatabaseName ?? SharedInMemoryDatabaseName;
            var databaseRoot = DatabaseRoot ?? SharedInMemoryDatabaseRoot;
            services.AddDbContextPool<GettingThingsDoneDbContext>(options => options.UseInMemoryDatabase(databaseName, databaseRoot));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfAsyncRepository<>));
            services.AddScoped<IActionService, ActionService>();
            services.AddScoped<IActionListService, ActionListService>();
            services.AddScoped<IProjectService, ProjectService>();

            services.AddScoped<ISomeSingletonService>(_ => new Mock<ISomeSingletonService>().Object);
            services.AddMemoryCache();
        }
    }
}
