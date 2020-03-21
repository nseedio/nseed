using DotNetCoreSeeds;
using GettingThingsDone.ApplicationCore.Services;
using GettingThingsDone.Contracts.Interface;
using GettingThingsDone.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Linq;

namespace GettingThingsDone.ApplicationCore.Tests.Unit
{
    public sealed class ObjectCreator
    {
        private readonly IServiceCollection serviceCollection;

        public ObjectCreator()
        {
            var services = new ServiceCollection();

            services.AddDbContextPool<GettingThingsDoneDbContext>(options => options.UseInMemoryDatabase("SharedUnitTestingInMemoryDatabase", SampleStartupForUnitTests.GlobalInMemoryDatabaseRoot));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfAsyncRepository<>));
            services.AddScoped<IActionService, ActionService>();
            services.AddScoped<IActionListService, ActionListService>();
            services.AddScoped<IProjectService, ProjectService>();

            services.AddScoped(_ => new Mock<ISomeSingletonService>().Object);
            services.AddMemoryCache();

            serviceCollection = services;
        }

        public ObjectCreator(IServiceCollection serviceCollection) => this.serviceCollection = serviceCollection;

        public ObjectCreator WithInMemoryDatabase()
        {
            RemoveRegisterdDbContextServices();

            serviceCollection.AddDbContextPool<GettingThingsDoneDbContext>(options => options.UseInMemoryDatabase("SharedUnitTestingInMemoryDatabase", SampleStartupForUnitTests.GlobalInMemoryDatabaseRoot));

            return this;
        }

        public ObjectCreator WithSqlServerDatabase()
        {
            RemoveRegisterdDbContextServices();

            serviceCollection.AddDbContextPool<GettingThingsDoneDbContext>(options => options.UseSqlServer($@"Server=(localdb)\mssqllocaldb;Database=SeedingWeedingOutAndDestroyingStartup2;Trusted_Connection=True;ConnectRetryCount=0"));

            return this;
        }

        public ObjectCreator With<TService>(TService implementation)
            where TService : class
        {
            serviceCollection.AddScoped(x => implementation);

            return this;
        }

        public TService Create<TService>()
        {
            return ActivatorUtilities.CreateInstance<TService>(serviceCollection.BuildServiceProvider());
        }

        private void RemoveRegisterdDbContextServices()
        {
            var descriptor = serviceCollection.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(DbContextPool<GettingThingsDoneDbContext>));
            if (descriptor != null) serviceCollection.Remove(descriptor);

            descriptor = serviceCollection.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(DbContextOptions));
            if (descriptor != null) serviceCollection.Remove(descriptor);

            descriptor = serviceCollection.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(DbContextOptions<GettingThingsDoneDbContext>));
            if (descriptor != null) serviceCollection.Remove(descriptor);

            descriptor = serviceCollection.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(DbContextOptions));
            if (descriptor != null) serviceCollection.Remove(descriptor);

            descriptor = serviceCollection.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(DbContextPool<GettingThingsDoneDbContext>.Lease));
            if (descriptor != null) serviceCollection.Remove(descriptor);

            descriptor = serviceCollection.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(GettingThingsDoneDbContext));
            if (descriptor != null) serviceCollection.Remove(descriptor);
        }
    }
}
