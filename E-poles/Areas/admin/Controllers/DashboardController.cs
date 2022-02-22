using AutoMapper;
using E_poles.Areas.admin.Models;
using E_poles.Dal;
using E_poles.Models.Pole;
using E_poles.services.Services;
using E_poles.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace E_poles.Areas.admin.Controllers
{
    [Route("Admin")]
    public class DashboardController : AdminBaseController
    {
        IUserService _userService;
        IGroupService _groupService;
        IEpoleService _epoleService;
        private readonly UserManager<User> _userManager;
        IDashboardService _dashboardService;
        private readonly IMapper _mapper;
        public DashboardController(IDashboardService dashboardService, IUserService userService, IGroupService groupService, UserManager<User> userManager, IEpoleService epoleService, IMapper mapper)
        {
            _mapper = mapper;
            _userManager = userManager;
            _groupService = groupService;
            _epoleService = epoleService;
            _dashboardService = dashboardService;
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            var cUser = await _userManager.GetUserAsync(User);
            var userGroups = await _groupService.GetGroupByUserId(cUser.Id);
            var poleList = await _epoleService.GetAll(userGroups.GroupsId);
            var adminLists = await _userService.GetAllFilterByRole(userGroups.GroupsId, Convert.ToInt32(RoleEnum.Administrator));
            var userLists = await _userService.GetAllFilterByRole(userGroups.GroupsId, Convert.ToInt32(RoleEnum.User));
            var data = await _dashboardService.GetAllInfomation();

            var model = new DashboardViewModel()
            {
                TotalAdmin = adminLists.Count(),
                TotalUser = userLists.Count(),
                TotalPole = poleList.Count()
            };
            return View(model);
        }
    }
}
