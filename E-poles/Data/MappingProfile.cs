using AutoMapper;
using E_poles.Dal;
using E_poles.Areas.admin.Models;

namespace E_poles.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Poles, PoleViewModel>().ReverseMap();
        }
    }
}
