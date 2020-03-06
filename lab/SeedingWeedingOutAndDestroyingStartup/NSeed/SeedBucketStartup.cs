using Microsoft.Extensions.DependencyInjection;
using NSeed.Abstractions;

namespace NSeed
{
    /// <summary>
    /// TODO.
    /// </summary>
    public abstract class SeedBucketStartup
    {
        // TODO: Think of this. Should it have state? No. How to ensure protocol? How to ensure passing of proper services? In general, we will have to think of service scopes etc.
        internal IServiceCollection CreateAndConfigureServiceCollection()
        {
            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            return serviceCollection;
        }

        internal void InitializeSeeding(IServiceCollection services, IOutputSink output) // TODO: Remove services from here or ensure protocol.
        {
            InitializeSeeding(services.BuildServiceProvider(), output);
        }

        /// <summary>
        /// TODO.
        /// </summary>
        /// <param name="services">TODO. TODO.</param>
        protected virtual void ConfigureServices(ServiceCollection services)
        {
        }

        /// <summary>
        /// TODO.
        /// </summary>
        /// <param name="serviceProvider">TODO. TODO.</param>
        /// <param name="output">TODO. TODO. X.</param>
        protected virtual void InitializeSeeding(ServiceProvider serviceProvider, IOutputSink output)
        {
        }
    }
}
