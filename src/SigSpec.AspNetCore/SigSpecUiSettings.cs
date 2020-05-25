using Microsoft.AspNetCore.Http;

namespace SigSpec.AspNetCore
{
    public class SigSpecUiSettings
    {
        public string Route { get; set; } = "/sigspec";

        public string TransformHtml(string html, HttpRequest request)
        {
            html = html.Replace("{Route}", this.Route);
            html = html.Replace("{BaseUrl}", request.Scheme + "://" + request.Host);
            return html;
        }
    }
}
