using Microsoft.AspNetCore.Mvc;

namespace Warranty.Web.Controllers
{
    public class UnauthorizedController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
