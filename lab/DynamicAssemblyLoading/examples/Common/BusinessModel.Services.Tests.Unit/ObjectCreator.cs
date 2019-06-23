using Microsoft.Extensions.DependencyInjection;

namespace BusinessModel.Services.Tests.Unit
{
    public sealed class ObjectCreator
    {
        private readonly IServiceCollection _serviceCollection;

        public ObjectCreator(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public ObjectCreator With<TService>(TService implementation) where TService : class
        {
            _serviceCollection.AddScoped(x => implementation);

            return this;
        }

        public TService Create<TService>()
        {
            return _serviceCollection.BuildServiceProvider().GetService<TService>();
        }
    }
}