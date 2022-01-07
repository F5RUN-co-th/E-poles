using AutoMapper;
using E_poles.Areas.admin.Models;
using E_poles.Dal;
using E_poles.Services;
using Microsoft.AspNetCore.Mvc;
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
                        return RedirectToAction(nameof(Create));
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
    }
}
