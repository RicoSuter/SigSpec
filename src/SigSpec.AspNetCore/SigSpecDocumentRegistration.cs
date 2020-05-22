using SigSpec.AspNetCore.Middlewares;
using SigSpec.Core;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>A SigSpec document generator registration.</summary>
    internal class SigSpecDocumentRegistration
    {
        private SigSpecDocumentGeneratorSettings _settings;

        /// <summary>Initializes a new instance of the <see cref="SigSpecDocumentRegistration"/> class.</summary>
        /// <param name="settings">The document settings.</param>
        public SigSpecDocumentRegistration(SigSpecDocumentGeneratorSettings settings)
        {
            _settings = settings;
        }

        /// <summary>Gets the document name.</summary>
        public string DocumentName => _settings.DocumentName;

        /// <summary>
        /// Generates the document.
        /// </summary>
        /// <returns>The JSON.</returns>
        public async Task<string> GenerateJsonAsync()
        {
            var generator = new SigSpecGenerator(_settings);
            var document = await generator.GenerateForHubsAsync(_settings.Hubs, _settings.Template);
            return document.ToJson();
        }
    }
}
