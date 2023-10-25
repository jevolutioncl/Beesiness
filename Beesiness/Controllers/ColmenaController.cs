using Microsoft.AspNetCore.Mvc;

namespace Beesiness.Controllers
{
    public class ColmenaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
