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
        private readonly IOutputSink output;

        public SampleStartupForUnitTests(IOutputSink output) // TODO: Validation: can only be parameterless or have IOutputSink as parameter.
        {
            this.output = output;
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            output.WriteVerboseMessage($"Executing {nameof(SampleStartupForUnitTests)}.{nameof(ConfigureServices)}");

            var inMemoryDatabaseRoot = new InMemoryDatabaseRoot();
            services.AddDbContextPool<GettingThingsDoneDbContext>(options => options.UseInMemoryDatabase("SharedUnitTestingInMemoryDatabase", inMemoryDatabaseRoot));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfAsyncRepository<>));
            services.AddScoped<IActionService, ActionService>();
            services.AddScoped<IActionListService, ActionListService>();
            services.AddScoped<IProjectService, ProjectService>();

            services.AddScoped<ISomeSingletonService>(_ => new Mock<ISomeSingletonService>().Object);
            services.AddMemoryCache();
        }
    }
}
