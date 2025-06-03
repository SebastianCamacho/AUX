using AutoMapper;
using BLL.DTOs;
using ENTITY;
using ENTITY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mappings
{
    public class Contracting_PartyMappingProfile : Profile
    {
        public Contracting_PartyMappingProfile()
        {

            CreateMap<Contracting_Party, Contracting_PartyDTO>();

            CreateMap<Contracting_PartyCreateDTO, Contracting_Party>()
                .ForMember(dest => dest.id_, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Contracting_PartyUpdateDTO, Contracting_Party>()
                // 1) Primero tu mapeo especial para Third_Party
                .ForMember(dest => dest.Third_Party, opt => opt.MapFrom((src, dest, _, ctx) =>
                    src.Third_Party == null
                        ? dest.Third_Party
                        : ctx.Mapper.Map(
                              src.Third_Party,
                              dest.Third_Party,
                              src.Third_Party.GetType(),
                              dest.Third_Party.GetType()
                          )
                ))
                // 2) Luego, ignoro todos los nulls en las demás props (incluye type_Owner, etc.)
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}
