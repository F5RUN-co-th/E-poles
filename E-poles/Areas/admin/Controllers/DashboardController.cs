using AutoMapper;
using E_poles.Areas.admin.Models;
using E_poles.Dal;
using E_poles.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace E_poles.Areas.admin.Controllers
{
    [Route("Admin")]
    public class DashboardController : AdminBaseController
    {
        IDashboardService _dashboardService;
        private readonly IMapper _mapper;
        public DashboardController(IDashboardService dashboardService, IMapper mapper)
        {
            _mapper = mapper;
            _dashboardService = dashboardService;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _dashboardService.GetAllInfomation();

            var model = new DashboardViewModel()
            {
                TotalAdmin = (int)data.GetType().GetProperty("TotalAdmin").GetValue(data),
                TotalUser = (int)data.GetType().GetProperty("TotalUser").GetValue(data),
                TotalPole = (int)data.GetType().GetProperty("TotalPole").GetValue(data)
            };
            return View(model);
        }
    }
}
