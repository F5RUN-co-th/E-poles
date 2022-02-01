using AutoMapper;
using E_poles.Dal;
using E_poles.Areas.admin.Models;
using E_poles.Models.Pole;

namespace E_poles.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Poles, PoleViewModel>().ReverseMap();
            CreateMap<Poles, PoleListModel>().ReverseMap();
        }
    }
}
