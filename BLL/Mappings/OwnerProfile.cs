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
    public class OwnerProfile : Profile
    {
        public OwnerProfile()
        {
            CreateMap<Owner, OwnerDTO>();
            CreateMap<OwnerDTO, Owner>();

            CreateMap<NaturalPersonDTO, Natural_Person>();
            CreateMap<LegalEntityDTO, Legal_Entity>();

            CreateMap<Natural_Person, NaturalPersonDTO>();
            CreateMap<Legal_Entity, LegalEntityDTO>();
        }
    }
}
