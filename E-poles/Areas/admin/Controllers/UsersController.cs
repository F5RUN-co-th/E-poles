using AutoMapper;
using E_poles.Areas.admin.Models;
using E_poles.Dal;
using E_poles.Models.Group;
using E_poles.Models.User;
using E_poles.services.Services;
using E_poles.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private IPasswordHasher<User> _passwordHasher;
        private IPasswordValidator<User> _passwordValidator;
        private readonly IRoles _roles;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public UsersController(IUserService userService, IPasswordHasher<User> passwordHash, IPasswordValidator<User> passwordVal, UserManager<User> userManager, IRoles roles, IGroupService groupService, IMapper mapper)
        {
            _mapper = mapper;
            _roles = roles;
            _passwordHasher = passwordHash;
            _passwordValidator = passwordVal;
            _userManager = userManager;
            _userService = userService;
            _groupService = groupService;
        }

        public IActionResult Index() => View();

        public async Task<IActionResult> Create()
        {
            var cUser = await _userManager.GetUserAsync(User);
            var model = new UserViewModel();
            var userGroups = await _groupService.GetGroupByUserId(cUser.Id);

            var groupList = await _groupService.GetAll(userGroups.GroupsId);
            model.GroupsList = groupList.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
            return View(model);
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userGroups = await _groupService.GetGroupByUserId(id ?? 0);
            var grp = await _userService.Get(id ?? 0);
            UserViewModel model = await ToUserModel(userGroups, grp);
            return View(model);
        }

        private async Task<UserViewModel> ToUserModel(UserGroups userGroups, User usr)
        {
            var roleId = await _roles.Get(usr.Id);
            var model = _mapper.Map<UserViewModel>(usr);
            var groupList = await _groupService.GetAll(userGroups.GroupsId);
            model.GroupsList = groupList.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
            model.SelectedGroup = userGroups.GroupsId.ToString();
            model.SelectedRole = roleId.ToString();
            return model;
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
                            UserGroups usergrp = new UserGroups
                            {
                                UserId = user.Id,
                                GroupsId = int.Parse(model.SelectedGroup)
                            };
                            await _groupService.CreateUserGroupsAsync(usergrp);
                            await _roles.AddToRoles(user.Id, (RoleEnum)int.Parse(model.SelectedRole));

                            UserViewModel newUser = await ToUserModel(userGroups, user);
                            return View("Update", newUser);
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
                    var user = await _userManager.FindByIdAsync(model.Id.ToString());
                    user.Email = model.Email.Trim();
                    user.UserName = model.UserName.Trim();
                    //var user = _mapper.Map<User>(model);
                    IdentityResult validPass = null;
                    if (!string.IsNullOrEmpty(model.PasswordHash))
                    {
                        validPass = await _passwordValidator.ValidateAsync(_userManager, user, model.PasswordHash);
                        if (validPass.Succeeded)
                        { user.PasswordHash = _passwordHasher.HashPassword(user, model.PasswordHash); }
                        else
                        {
                            foreach (IdentityError error in validPass.Errors)
                                ModelState.AddModelError("", error.Description);
                        }
                    }
                    else { ModelState.AddModelError("", "Password cannot be empty"); }

                    var makerUsrGps = await _groupService.GetGroupByUserId(int.Parse(model.UserId));
                    var allUsers = await _userService.GetAll(makerUsrGps.GroupsId);
                    if (!allUsers.Any(s => s.UserName.ToLower() == user.UserName.ToLower() && user.Id != s.Id))
                    {
                        var result = await _userService.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            var userGroups = await _groupService.GetGroupByUserId(user.Id);
                            await _groupService.DeleteUserGroupsAsync(userGroups);
                            UserGroups newUsrGroup = new UserGroups
                            {
                                UserId = user.Id,
                                GroupsId = int.Parse(model.SelectedGroup)
                            };
                            await _groupService.CreateUserGroupsAsync(newUsrGroup);

                            var role = await _userManager.GetRolesAsync(user);
                            await _roles.RemoveFromRoles(user.Id, role);
                            await _roles.AddToRoles(user.Id, (RoleEnum)int.Parse(model.SelectedRole));

                            UserViewModel newUser = await ToUserModel(userGroups, user);
                            return View("Update", newUser);
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
