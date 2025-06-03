using AutoMapper;
using BLL.DTOs;
using ENTITY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mappings
{
    public class Legal_EntityMappingProfile : Profile
    {
        public Legal_EntityMappingProfile()
        {
            CreateMap<Legal_Entity, Legal_EntityDTO>().ReverseMap()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));

            CreateMap<Legal_EntityCreateDTO, Legal_Entity>().ReverseMap();

            CreateMap<Legal_EntityUpdateDTO, Legal_Entity>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Legal_Entity, Legal_EntityUpdateDTO>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));



        }
    }
}
