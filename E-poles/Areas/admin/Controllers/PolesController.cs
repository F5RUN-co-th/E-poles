using AutoMapper;
using E_poles.Areas.admin.Models;
using E_poles.Dal;
using E_poles.Models.Datables;
using E_poles.Models.Pole;
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
    public class PolesController : AdminBaseController
    {
        IGroupService _groupService;
        IEpoleService _epoleService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public PolesController(IGroupService groupService, UserManager<User> userManager, IEpoleService epoleService, IMapper mapper)
        {
            _mapper = mapper;
            _userManager = userManager;
            _groupService = groupService;
            _epoleService = epoleService;
        }

        public async Task<IActionResult> Index()
        {
            var cUser = await _userManager.GetUserAsync(User);
            var userGroups = await _groupService.GetGroupByUserId(cUser.Id);
            var model = new SrchPolesModel();
            var areaList = await _epoleService.GetAllArea(userGroups.GroupsId);
            model.AreaList = areaList.Select(x => new SelectListItem
            {
                Value = x.Area,
                Text = x.Area
            }).ToList();
            var streetList = await _epoleService.GetAllStreet(userGroups.GroupsId);
            model.StreetList = streetList.Select(x => new SelectListItem
            {
                Value = x.Street,
                Text = x.Street
            }).ToList();
            return View(model);
        }

        public IActionResult Create() => View();

        public async Task<IActionResult> GetDtPolesList([FromBody] SrchPolesModel model)
        {
            try
            {
                var result = await GetAllPolesByGroupId(model.UserId);

                var poleList = _mapper.Map<IEnumerable<PoleListModel>>(result);
                if (!String.IsNullOrEmpty(model.SelectedArea))
                {
                    poleList = poleList.Where(s =>
                                         (s.Area != null && s.Area.Contains(model.SelectedArea))
                                        );
                }
                if (!String.IsNullOrEmpty(model.SelectedStreet))
                {
                    poleList = poleList.Where(s =>
                                        (s.Street != null && s.Street.Contains(model.SelectedStreet)));
                }
                if (!String.IsNullOrEmpty(model.SelectedStatus))
                {
                    var _status = Convert.ToBoolean(int.Parse(model.SelectedStatus));
                    poleList = poleList.Where(w => w.Status == _status);
                }
                if (!String.IsNullOrEmpty(model.KeySearch))
                {
                    poleList = poleList.Where(s =>
                                        (s.Name != null && s.Name.ToLower().Contains(model.KeySearch.ToLower())) ||
                                        (s.Area != null && s.Area.Contains(model.KeySearch)) ||
                                        (s.Street != null && s.Street.Contains(model.KeySearch)) ||
                                        (s.Note != null && s.Note.ToLower().Contains(model.KeySearch.ToLower())) ||
                                        (s.Description != null && s.Description.ToLower().Contains(model.KeySearch.ToLower()))
                                        );
                }

                int recordsTotal = poleList.Count();
                var data = poleList.Skip(model.Start).Take(model.Length);
                var jsonData = new { draw = model.Draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                return Ok();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPoles(int userId)
        {
            var userGroups = await _groupService.GetGroupByUserId(userId);

            var result = await _epoleService.GetAll(userGroups.GroupsId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetLastStreetArea(int userId)
        {
            var userGroups = await _groupService.GetGroupByUserId(userId);

            var result = await _epoleService.GetLast(userGroups.GroupsId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePoles(PoleViewModel model)
        {
            this.ModelState.Remove("Id");
            try
            {
                if (ModelState.IsValid)
                {
                    model.Name = model.Name.Trim();
                    model.Area = model.Area.Trim();
                    model.Street = model.Street.Trim();
                    if (model.Note != null)
                        model.Note = model.Note.Trim();
                    if (model.Description != null)
                        model.Description = model.Description.Trim();

                    int userId = int.Parse(model.UserId);
                    var userGroups = await _groupService.GetGroupByUserId(userId);
                    var pole = _mapper.Map<Poles>(model);
                    pole.GroupsId = userGroups.GroupsId;
                    pole.CreatedAt = DateTime.Now;
                    pole.CreatedBy = userId;
                    pole.UpdatedAt = DateTime.Now;
                    pole.UpdatedBy = userId;
                    var result = await _epoleService.CreateAsync(pole);
                    if (result != null)
                    {
                        //var ok = new BaseResponse<Poles>
                        //{
                        //    Message = "Sucess",
                        //    StatusCode = 200
                        //};
                        //return RedirectToAction(nameof(Create));
                        return Ok(await GetAllPolesByGroupId(model.UserId));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid request input.");
                        return View("Create", model);
                    }
                }
            }
            catch (System.Exception ex)
            {

            }
            return View("Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePoles(PoleViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Name = model.Name.Trim();
                    model.Area = model.Area.Trim();
                    model.Street = model.Street.Trim();
                    if (model.Note != null)
                        model.Note = model.Note.Trim();
                    if (model.Description != null)
                        model.Description = model.Description.Trim();

                    int userId = int.Parse(model.UserId);
                    var userGroups = await _groupService.GetGroupByUserId(userId);
                    var pole = _mapper.Map<Poles>(model);
                    pole.GroupsId = userGroups.GroupsId;
                    pole.UpdatedAt = DateTime.Now;
                    pole.UpdatedBy = userId;
                    var result = await _epoleService.UpdateAsync(pole);
                    if (result)
                    {
                        return Ok(await GetAllPolesByGroupId(model.UserId));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid request input.");
                        return View("Create", model);
                    }
                }
            }
            catch (System.Exception ex)
            {

            }
            return View("Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDtPoles([FromBody] PoleViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Name = model.Name.Trim();
                    model.Area = model.Area.Trim();
                    model.Street = model.Street.Trim();
                    if (model.Note != null)
                        model.Note = model.Note.Trim();
                    if (model.Description != null)
                        model.Description = model.Description.Trim();

                    int userId = int.Parse(model.UserId);
                    var userGroups = await _groupService.GetGroupByUserId(userId);
                    var pole = _mapper.Map<Poles>(model);
                    pole.GroupsId = userGroups.GroupsId;
                    pole.UpdatedAt = DateTime.Now;
                    pole.UpdatedBy = userId;
                    var result = await _epoleService.UpdateAsync(pole);
                    if (result)
                    {
                        return Ok(await GetAllPolesByGroupId(model.UserId));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid request input.");
                        return View("Create", model);
                    }
                }
            }
            catch (System.Exception ex)
            {

            }
            return View("Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePoles([FromBody] PoleViewModel data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var pole = _mapper.Map<Poles>(data);
                    var result = await _epoleService.DeleteAsync(pole);
                    if (result)
                    {
                        return Ok(await GetAllPolesByGroupId(data.UserId));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid request input.");
                        return View("Create", data);
                    }
                }
            }
            catch (System.Exception ex)
            {
                return Ok(ex.Message);
            }
            return Ok(data);
        }

        private async Task<IEnumerable<Poles>> GetAllPolesByGroupId(string userId)
        {
            var userGroups = await _groupService.GetGroupByUserId(int.Parse(userId));

            var result = await _epoleService.GetAll(userGroups.GroupsId);
            return result;
        }
    }
}
