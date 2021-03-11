using System.Threading;
using SigSpec.AspNetCore.Middlewares;
using SigSpec.Core;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>A SigSpec _document generator registration.</summary>
    internal class SigSpecDocumentRegistration
    {
        static SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1,1);
        private SigSpecDocumentGeneratorSettings _settings;
        private SigSpecDocument _document;
        /// <summary>Initializes a new instance of the <see cref="SigSpecDocumentRegistration"/> class.</summary>
        /// <param name="settings">The _document settings.</param>
        public SigSpecDocumentRegistration(SigSpecDocumentGeneratorSettings settings)
        {
            _settings = settings;
        }

        /// <summary>Gets the _document name.</summary>
        public string DocumentName => _settings.DocumentName;

        /// <summary>
        /// Generates the _document.
        /// </summary>
        /// <returns>The JSON.</returns>
        public async Task<string> GenerateJsonAsync()
        {
            var generator = new SigSpecGenerator(_settings);
            await _semaphoreSlim.WaitAsync();
            try
            {
                if (_document==null)
                    _document = await generator.GenerateForHubsAsync(_settings.Hubs, _settings.Template);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
                
            return _document.ToJson();
        }
    }
}
