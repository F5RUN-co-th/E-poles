using AutoMapper;
using E_poles.Areas.admin.Models;
using E_poles.Models.Pole;
using E_poles.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_poles.Controllers
{
    public class PolesController : Controller
    {
        IEpoleService _epoleService;
        private readonly IMapper _mapper;
        public PolesController(IEpoleService epoleService, IMapper mapper)
        {
            _mapper = mapper;
            _epoleService = epoleService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new SrchPolesModel();
            var areaList = await _epoleService.GetAllArea();
            model.AreaList = areaList.Select(x => new SelectListItem
            {
                Value = x.Area,
                Text = x.Area
            }).ToList();
            var streetList = await _epoleService.GetAllStreet();
            model.StreetList = streetList.Select(x => new SelectListItem
            {
                Value = x.Street,
                Text = x.Street
            }).ToList();
            return View(model);
        }

        public async Task<IActionResult> GetDtPolesList([FromBody] SrchPolesModel model)
        {
            var result = await _epoleService.GetAll();

            var poleList = _mapper.Map<IEnumerable<PoleListModel>>(result);
            if (!String.IsNullOrEmpty(model.KeySearch))
            {
                poleList = poleList.Where(s =>
                                    s.FullName.Contains(model.KeySearch) ||
                                    s.Name.Contains(model.KeySearch) ||
                                    s.Area.Contains(model.KeySearch) ||
                                    s.Street.Contains(model.KeySearch) ||
                                    (s.Note != null && s.Note.Contains(model.KeySearch)) ||
                                    (s.Description != null && s.Description.Contains(model.KeySearch)));
            }
            if (!String.IsNullOrEmpty(model.SelectedArea))
            {
                poleList = poleList.Where(s =>
                                    s.FullName.Contains(model.SelectedArea) ||
                                    s.Area.Contains(model.SelectedArea));
            }
            if (!String.IsNullOrEmpty(model.SelectedStreet))
            {
                poleList = poleList.Where(s =>
                                    s.FullName.Contains(model.SelectedStreet) ||
                                    s.Street.Contains(model.SelectedStreet));
            }
            if (!String.IsNullOrEmpty(model.SelectedStatus))
            {
                var _status = Convert.ToBoolean(int.Parse(model.SelectedStatus));
                poleList = poleList.Where(w => w.Status == _status);
            }
            int recordsTotal = poleList.Count();
            var data = poleList.Skip(model.Start).Take(model.Length);
            var jsonData = new { draw = model.Draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return Ok(jsonData);
            //return Json(new DtResult<Poles>
            //{
            //    Draw = dtParameters.Draw,
            //    RecordsTotal = result.Count(),
            //    RecordsFiltered = result.Count(),
            //    Data = result.Skip(dtParameters.Start)
            //        .Take(dtParameters.Length)
            //});
        }

    }
}
