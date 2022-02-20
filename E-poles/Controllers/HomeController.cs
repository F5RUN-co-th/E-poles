using E_poles.Dal;
using E_poles.Models;
using E_poles.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace E_poles.Controllers
{
    [Authorize]
    [Produces("application/json")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IGroupService _groupService;
        IEpoleService _epoleService;
        public HomeController(ILogger<HomeController> logger, IGroupService groupService, IEpoleService epoleService)
        {
            _logger = logger;
            _groupService = groupService;
            _epoleService = epoleService;
        }

        public IActionResult Index() => View();


        [HttpGet]
        public async Task<IActionResult> GetAllPoles(int userId)
        {
            return Ok(await GetAllPolesByGroupId(userId));
        }

        private async Task<IEnumerable<Poles>> GetAllPolesByGroupId(int userId)
        {
            var userGroups = await _groupService.GetGroupByUserId(userId);

            var result = await _epoleService.GetAll(userGroups.GroupsId);
            return result;
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
