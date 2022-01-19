using AutoMapper;
using E_poles.Areas.admin.Models;
using E_poles.Dal;
using E_poles.Models.Datables;
using E_poles.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace E_poles.Areas.admin.Controllers
{
    public class PolesController : AdminBaseController
    {
        IEpoleService _epoleService;
        private readonly IMapper _mapper;
        public PolesController(IEpoleService epoleService, IMapper mapper)
        {
            _mapper = mapper;
            _epoleService = epoleService;
        }

        public IActionResult Index() => View();

        public IActionResult Create() => View();

        public async Task<IActionResult> GetDtPolesList([FromBody] DtParameters dtParameters, SrchPolesModel searchModel)
        {
            var result = await _epoleService.GetAll();
            return Json(new DtResult<Poles>
            {
                Draw = dtParameters.Draw,
                RecordsTotal = result.Count(),
                RecordsFiltered = result.Count(),
                Data = result.Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPoles() => Ok(await _epoleService.GetAll());

        [HttpPost]
        public async Task<IActionResult> CreatePoles(PoleViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var pole = _mapper.Map<Poles>(model);
                    var result = await _epoleService.CreateAsync(pole);
                    if (result != null)
                    {
                        //var ok = new BaseResponse<Poles>
                        //{
                        //    Message = "Sucess",
                        //    StatusCode = 200
                        //};
                        //return RedirectToAction(nameof(Create));
                        return Ok(await _epoleService.GetAll());
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
                        return Ok(await _epoleService.GetAll());
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
    }
}
