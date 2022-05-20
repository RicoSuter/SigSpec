using SigSpec.AspNetCore.Middlewares;
using SigSpec.Core;
using System;
using System.Collections.Generic;
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

        /// <summary>Gets the output path.</summary>
        public string OutputPath => _settings.OutputPath;

        public Action<SigSpecDocument> CommandLineAction => _settings.CommandLineAction;

        public Dictionary<string, Type> Hubs => _settings.Hubs;

        /// <summary>
        /// Generates the document.
        /// </summary>
        /// <returns>The JSON.</returns>
        public async Task<SigSpecDocument> GenerateDocumentAsync()
        {
            var generator = new SigSpecGenerator(_settings);
            return await generator.GenerateForHubsAsync(_settings.Hubs, _settings.Template);
        }
    }
}
