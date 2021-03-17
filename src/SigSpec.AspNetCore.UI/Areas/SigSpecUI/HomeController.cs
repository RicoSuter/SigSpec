using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace SigSpec.AspNetCore.UI.Areas.SigSpecUI
{
    [Area("SigSpecUi")]
    public class HomeController : Controller
    {
        private readonly IEnumerable<SigSpecDocumentRegistration> documents;

        public HomeController(IEnumerable<SigSpecDocumentRegistration> documents)
        {
            this.documents = documents;
        }
        public IActionResult Index([FromQuery]string ver)
        {
            if (string.IsNullOrEmpty(ver))
                ver = documents.First().DocumentName;
            var model = documents.Select(t =>
                new SelectListItem(t.DocumentName, t.DocumentName, t.DocumentName == ver)).ToList();
            return View(model);
        }
    }
}
