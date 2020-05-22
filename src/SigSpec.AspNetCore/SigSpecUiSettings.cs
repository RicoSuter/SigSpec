using Microsoft.AspNetCore.Http;

namespace SigSpec.AspNetCore
{
    public class SigSpecUiSettings
    {
        public string Route { get; set; } = "/sigspec";

        public string TransformHtml(string html, HttpRequest contextRequest)
        {
            return html;
        }
    }
}
