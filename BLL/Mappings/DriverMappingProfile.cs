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
    public class DriverMappingProfile : Profile
    {
        public DriverMappingProfile()
        {
            // DriverCreateDTO -> Driver (Cédula y Estado)
            CreateMap<DriverCreateDTO, Driver>()
               .ForMember(dest => dest.state, opt => opt.Ignore()) // Se establece en el servicio
               .ForMember(dest => dest.create_At, opt => opt.Ignore()) // Se establece en el servicio
               .ForMember(dest => dest.update_At, opt => opt.Ignore()) // Se establece en el servicio
               .ForMember(dest => dest.create_By, opt => opt.Ignore()) // Se establece en el servicio
               .ForMember(dest => dest.update_By, opt => opt.Ignore()) // Se establece en el servicio
               .ForMember(dest => dest.Document_Drivers, opt => opt.Ignore()); // Se manejan por separado

            // Mapeo para Actualizar: DriverUpdateDTO -> Driver
            CreateMap<DriverUpdateDTO, Driver>()
             .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Mapeo para Leer: Driver -> DriverDTO
            // AutoMapper puede mapear la colección Document_Drivers a Documents (List<DocumentDTO>)
            // si ya existe un mapeo definido para Document_Driver -> DocumentDTO en DocumentMappingProfile.
            CreateMap<Driver, DriverDTO>();



            //Mapeo HISTORY: Desde la entidad Driver hacia la entidad Driver_History
            CreateMap<Driver, Driver_History>()
                // Ignoramos los campos que se llenarán específicamente para el registro de historial
                .ForMember(dest => dest.HistoryId, opt => opt.Ignore()) // Es la PK autoincremental de Driver_History
                .ForMember(dest => dest.DriverAuditId, opt => opt.Ignore()) // Se asignará desde Driver.driver_Id en el servicio
                .ForMember(dest => dest.ActionTimestamp, opt => opt.Ignore()) // Se asignará en el servicio
                .ForMember(dest => dest.ActionType, opt => opt.Ignore()) // Se asignará en el servicio
                .ForMember(dest => dest.ChangedByUserId, opt => opt.Ignore()) // Se asignará en el servicio
                                                                              // Mapeamos los campos de auditoría originales del Driver a los campos con sufijo "_OriginalDriver"
                .ForMember(dest => dest.create_At_OriginalDriver, opt => opt.MapFrom(src => src.create_At))
                .ForMember(dest => dest.update_At_OriginalDriver, opt => opt.MapFrom(src => src.update_At))
                .ForMember(dest => dest.create_By_OriginalDriver, opt => opt.MapFrom(src => src.create_By))
                .ForMember(dest => dest.update_By_OriginalDriver, opt => opt.MapFrom(src => src.update_By));
            // El resto de los campos (type_Id, image, first_Name, state, etc.)
            // se mapearán automáticamente por AutoMapper si los nombres coinciden,

        }
    }
}
