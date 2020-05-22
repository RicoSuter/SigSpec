using System;
using SigSpec.AspNetCore.Middlewares;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>NSwag extensions for <see cref="IServiceCollection"/>.</summary>
    public static class SigSpecServiceCollectionExtensions
    {
        /// <summary>Adds a SigSpec document.</summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configure">Configure the document.</param>
        public static IServiceCollection AddSigSpecDocument(this IServiceCollection serviceCollection, Action<SigSpecDocumentGeneratorSettings> configure)
        {
            return serviceCollection.AddSigSpecDocument((s, p) => configure(s));
        }

        /// <summary>Adds a SigSpec document.</summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configure">Configure the document.</param>
        public static IServiceCollection AddSigSpecDocument(this IServiceCollection serviceCollection, Action<SigSpecDocumentGeneratorSettings, IServiceProvider> configure = null)
        {
            serviceCollection.AddSingleton(services =>
            {
                var settings = new SigSpecDocumentGeneratorSettings();
                configure?.Invoke(settings, services);

                return new SigSpecDocumentRegistration(settings);
            });

            return serviceCollection;
        }
    }
}
