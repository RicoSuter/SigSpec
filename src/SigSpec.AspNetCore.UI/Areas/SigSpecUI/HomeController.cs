using Microsoft.AspNetCore.Mvc;

namespace SigSpec.AspNetCore.UI.Areas.SigSpecUI
{
    [Area("SigSpecUi")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
