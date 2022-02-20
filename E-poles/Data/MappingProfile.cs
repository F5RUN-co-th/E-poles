using AutoMapper;
using E_poles.Dal;
using E_poles.Areas.admin.Models;
using E_poles.Models.Pole;
using E_poles.Models.Group;

namespace E_poles.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Poles, PoleViewModel>().ReverseMap();
            CreateMap<Poles, PoleListModel>().ReverseMap();
            CreateMap<Groups, GroupViewModel>().ReverseMap();
            CreateMap<Groups, GroupListModel>().ReverseMap();
        }
    }
}
