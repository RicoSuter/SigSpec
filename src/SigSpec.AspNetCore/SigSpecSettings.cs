using Microsoft.AspNetCore.Http;

namespace SigSpec.AspNetCore.Middlewares
{
    public class SigSpecSettings
    {
        /// <summary>Gets or sets the path to serve the SigSpec document (default: '/sigspec/{documentName}/sigspec.json').</summary>
        public string Path { get; set; } = "/sigspec/{documentName}/sigspec.json";

        public string TransformHtml(string html, HttpRequest contextRequest)
        {
            return html;
        }
    }
}
