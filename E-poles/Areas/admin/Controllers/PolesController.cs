using E_poles.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_poles.Areas.admin.Controllers
{
    public class PolesController : AdminBaseController
    {
        IEpoleService _epoleService;
        public PolesController(IEpoleService epoleService)
        {
            _epoleService = epoleService;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetAllPoles() => Ok(await _epoleService.GetAll());

    }
}
