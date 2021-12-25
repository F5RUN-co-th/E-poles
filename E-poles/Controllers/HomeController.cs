using E_poles.Models;
using E_poles.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace E_poles.Controllers
{
    [Authorize]
    [Produces("application/json")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IEpoleService _epoleService;
        public HomeController(ILogger<HomeController> logger, IEpoleService epoleService)
        {
            _logger = logger;
            _epoleService = epoleService;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetAllPoles() => Ok(await _epoleService.GetAll());

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
