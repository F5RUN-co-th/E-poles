using AutoMapper;
using E_poles.Areas.admin.Models;
using E_poles.Dal;
using E_poles.Models.Group;
using E_poles.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_poles.Areas.admin.Controllers
{
    public class GroupsController : AdminBaseController
    {
        IGroupService _groupService;
        private readonly IMapper _mapper;
        public GroupsController(IGroupService groupService, IMapper mapper)
        {
            _mapper = mapper;
            _groupService = groupService;
        }

        public IActionResult Index() => View();

        public IActionResult Create() => View();

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var grp = await _groupService.Get(id ?? 0);
            return View(_mapper.Map<GroupViewModel>(grp));
        }

        public async Task<IActionResult> GetDtGroupsList([FromBody] SrchGroupModel model)
        {
            var userGroups = await _groupService.GetGroupByUserId(int.Parse(model.UserId));

            var result = await _groupService.GetAll(userGroups.GroupsId);

            var groupList = _mapper.Map<IEnumerable<GroupListModel>>(result);
            if (!String.IsNullOrEmpty(model.KeySearch))
            {
                groupList = groupList.Where(s => s.Name.ToLower().Contains(model.KeySearch.ToLower()));
            }

            int recordsTotal = groupList.Count();
            var data = groupList.Skip(model.Start).Take(model.Length);
            var jsonData = new { draw = model.Draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return Ok(jsonData);
        }

        [HttpPost]
        public async Task<IActionResult> Create(GroupViewModel model)
        {
            this.ModelState.Remove("Id");
            try
            {
                if (ModelState.IsValid)
                {
                    model.Name = model.Name.Trim();
                    var userGroups = await _groupService.GetGroupByUserId(int.Parse(model.UserId));
                    var allGroups = await _groupService.GetAll(userGroups.GroupsId);
                    if (!allGroups.Any(s => s.Name.ToLower() == model.Name.ToLower()))
                    {
                        var group = _mapper.Map<Groups>(model);
                        var result = await _groupService.CreateAsync(group);
                        if (result != null)
                        {
                            return View("Update", _mapper.Map<GroupViewModel>(result));
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid request input.");
                            return View("Create", model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"Group name {model.Name} already exists.");
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
        public async Task<IActionResult> Update(GroupViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Name = model.Name.Trim();
                    var userGroups = await _groupService.GetGroupByUserId(int.Parse(model.UserId));
                    var allGroups = await _groupService.GetAll(userGroups.GroupsId);
                    if (!allGroups.Any(s => s.Name.ToLower() == model.Name.ToLower() && model.Id != s.Id))
                    {
                        var group = _mapper.Map<Groups>(model);
                        var result = await _groupService.UpdateAsync(group);
                        if (result)
                        {
                            var grp = await _groupService.Get(model.Id);
                            return View("Update", _mapper.Map<GroupViewModel>(grp));
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid request input.");
                            return View("Update", model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"Group name {model.Name} already exists.");
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

        [HttpPost]
        public async Task<IActionResult> DeleteGroup([FromBody] GroupViewModel data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //var pole = _mapper.Map<Poles>(data);
                    //var result = await _groupService.DeleteAsync(pole);
                    //if (result)
                    //{
                    //    return Ok(await _groupService.GetAll());
                    //}
                    //else
                    //{
                    //    ModelState.AddModelError(string.Empty, "Invalid request input.");
                    //    return View("Create", data);
                    //}
                }
            }
            catch (System.Exception ex)
            {
                return Ok(ex.Message);
            }
            return Ok(data);
        }
    }
}
