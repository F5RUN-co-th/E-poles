using AutoMapper;
using E_poles.Areas.admin.Models;
using E_poles.Dal;
using E_poles.Models.Group;
using E_poles.Models.User;
using E_poles.services.Services;
using E_poles.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_poles.Areas.admin.Controllers
{
    public class UsersController : AdminBaseController
    {
        IUserService _userService;
        IGroupService _groupService;
        private readonly IMapper _mapper;
        public UsersController(IUserService userService, IGroupService groupService, IMapper mapper)
        {
            _mapper = mapper;
            _userService = userService;
            _groupService = groupService;
        }

        public IActionResult Index() => View();

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var grp = await _userService.Get(id ?? 0);
            return View(_mapper.Map<UserViewModel>(grp));
        }

        public async Task<IActionResult> GetDtUsersList([FromBody] SrchUserModel model)
        {
            var userGroups = await _groupService.GetGroupByUserId(int.Parse(model.UserId));

            var result = await _userService.GetAll(userGroups.GroupsId);

            var userLists = _mapper.Map<IEnumerable<UserListModel>>(result);
            if (!String.IsNullOrEmpty(model.KeySearch))
            {
                userLists = userLists.Where(s => s.UserName.ToLower().Contains(model.KeySearch.ToLower()));
            }

            int recordsTotal = userLists.Count();
            var data = userLists.Skip(model.Start).Take(model.Length);
            var jsonData = new { draw = model.Draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return Ok(jsonData);
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            this.ModelState.Remove("Id");
            try
            {
                if (ModelState.IsValid)
                {
                    model.Email = model.Email.Trim();
                    model.UserName = model.UserName.Trim();
                    var userGroups = await _groupService.GetGroupByUserId(int.Parse(model.UserId));
                    var allUsers = await _userService.GetAll(userGroups.GroupsId);
                    if (!allUsers.Any(s => s.UserName.ToLower() == model.UserName.ToLower()))
                    {
                        var user = _mapper.Map<User>(model);
                        var result = await _userService.CreateAsync(user, model.PasswordHash);
                        if (result.Succeeded)
                        {
                            return View("Update", _mapper.Map<UserViewModel>(result));
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid request input.");
                            return View("Create", model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"User name {model.UserName} already exists.");
                        return View("Create", model);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid request input.");
                    return View("Create", model);
                }
            }
            catch (System.Exception ex)
            {

            }
            return View("Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.UserName = model.UserName.Trim();
                    var userGroups = await _groupService.GetGroupByUserId(int.Parse(model.UserId));
                    var allUsers = await _userService.GetAll(userGroups.GroupsId);
                    if (!allUsers.Any(s => s.UserName.ToLower() == model.UserName.ToLower() && model.Id != s.Id))
                    {
                        var user = _mapper.Map<User>(model);
                        var result = await _userService.UpdateAsync(user);
                        if (result)
                        {
                            var usr = await _userService.Get(model.Id);
                            return View("Update", _mapper.Map<UserViewModel>(usr));
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid request input.");
                            return View("Update", model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"User name {model.UserName} already exists.");
                        return View("Update", model);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid request input.");
                    return View("Update", model);
                }
            }
            catch (System.Exception ex)
            {

            }
            return View("Update", model);
        }
    }
}
