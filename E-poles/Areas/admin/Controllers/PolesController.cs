using Microsoft.AspNetCore.Mvc;

namespace E_poles.Areas.admin.Controllers
{
    public class PolesController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
