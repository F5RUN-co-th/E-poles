using Microsoft.AspNetCore.Mvc;

namespace E_poles.Areas.admin.Controllers
{
    [Route("Admin")]
    public class DashboardController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
