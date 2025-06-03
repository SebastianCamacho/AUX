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
    public class Natural_PersonProfile : Profile
    {
        public Natural_PersonProfile()
        {
            CreateMap<Natural_Person, Natural_PersonDTO>().ReverseMap()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));
            CreateMap<Natural_PersonCreateDTO, Natural_Person>().ReverseMap();
            CreateMap<Natural_PersonUpdateDTO, Natural_Person>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

}
