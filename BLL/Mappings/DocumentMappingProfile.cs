using AutoMapper;
using BLL.DTOs;
using ENTITY.Models;
using ENTITY.Models_Global;
using ENTITY.Models_Histories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mappings
{
    public class DocumentMappingProfile : Profile
    {
        public DocumentMappingProfile()
        {
            CreateMap<DocumentCreateDTO, Document_Driver>();
            CreateMap<DocumentCreateDTO, Document_Driver_Global>();

            
            CreateMap<Document_Driver, DocumentDTO>();
            CreateMap<Document_Driver_Global, DocumentDTO>();

            CreateMap<DocumentUpdateDTO, Document_Driver>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<DocumentUpdateDTO, Document_Driver_Global>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // --- HISTORY Mapeo: Document_Driver -> Document_Driver_History ---
            CreateMap<Document_Driver, Document_Driver_History>()
            .ForMember(dest => dest.DocHistoryId, opt => opt.Ignore()) // PK autoincremental
            .ForMember(dest => dest.DocumentAuditId, opt => opt.MapFrom(src => src.id_Document)) // ID original del documento
            .ForMember(dest => dest.DriverHistoryId_FK, opt => opt.Ignore()) // Se asigna manualmente en el servicio
            .ForMember(dest => dest.ActionTimestamp, opt => opt.Ignore())   // Se asigna manualmente
            .ForMember(dest => dest.ActionType, opt => opt.Ignore())        // Se asigna manualmente
            .ForMember(dest => dest.ChangedByUserId, opt => opt.Ignore())   // Se asigna manualmente
            .ForMember(dest => dest.driver_Id_Original, opt => opt.MapFrom(src => src.driver_Id)); // Cédula del driver original
            // Las demás propiedades con nombres coincidentes (type_Document, start_validity, etc.)
            // se mapearán automáticamente por AutoMapper.
        }
    }
}
