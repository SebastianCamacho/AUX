using AutoMapper;
using BLL.DTOs;
using ENTITY.Models;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mappings
{
    
    public class Third_PartyMappingProfile : Profile
    {
        public Third_PartyMappingProfile()
        {
            // Mapeo desde la entidad hacia el DTO (response)
            CreateMap<Third_Party, Third_PartyDTO>()
                .Include<Natural_Person, Natural_PersonDTO>()
                .Include<Legal_Entity, Legal_EntityDTO>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));


            CreateMap<Third_PartyCreateDTO, Third_Party>()
                .ConvertUsing((src, dest, context) =>
                {
                    return src.type_Third_Party switch
                    {
                        "Persona Natural" => context.Mapper.Map<Natural_Person>(src as Natural_PersonCreateDTO),
                        "Persona Juridica" => context.Mapper.Map<Legal_Entity>(src as Legal_EntityCreateDTO),
                        _ => throw new AutoMapperMappingException("Tipo de tercero no reconocido")
                    };
                });

            CreateMap<Third_PartyUpdateDTO, Third_Party>()
                .ConvertUsing((src, dest, context) =>
                {
                    return src.type_Third_Party switch
                    {
                        "Persona Natural" => context.Mapper.Map<Natural_Person>(src as Natural_PersonUpdateDTO),
                        "Persona Juridica" => context.Mapper.Map<Legal_Entity>(src as Legal_EntityUpdateDTO),
                        _ => throw new AutoMapperMappingException("Tipo de tercero no reconocido")
                    };
                });
                



        }
    }

}
