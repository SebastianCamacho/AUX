using AutoMapper;
using BLL.DTOs;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mappings
{
    public class AddressMappingProfile : Profile
    {
        public AddressMappingProfile()
        {
            // Mapeo de Address → AddressDTO
            CreateMap<Address, AddressDTO>();

            // Mapeo de AddressCreateDTO → Address
            CreateMap<AddressCreateDTO, Address>()
                .ForMember(dest => dest.id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<AddressUpdateDTO, Address>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
              
        }
    }
}
