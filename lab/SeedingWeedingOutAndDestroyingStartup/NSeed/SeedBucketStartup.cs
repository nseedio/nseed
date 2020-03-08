using Microsoft.Extensions.DependencyInjection;
using System;

namespace NSeed
{
    /// <summary>
    /// TODO.
    /// </summary>
    public abstract class SeedBucketStartup
    {
        /// <summary>
        /// TODO.
        /// </summary>
        /// <param name="services">TODO. TODO.</param>
        protected internal virtual void ConfigureServices(IServiceCollection services)
        {
        }

        /// <summary>
        /// TODO.
        /// </summary>
        /// <param name="serviceProvider">TODO. TODO.</param>
        protected internal virtual void InitializeSeeding(IServiceProvider serviceProvider)
        {
        }
    }
}
