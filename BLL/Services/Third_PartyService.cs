using AutoMapper;
using BLL.DTOs;
using BLL.Exceptions;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Repositories;
using ENTITY;
using ENTITY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class Third_PartyService : IThird_PartyService
    {
        private readonly IThird_PartyRepository _repository;
        private readonly IMapper _mapper;
        public Third_PartyService(IThird_PartyRepository third_PartyRepository, IMapper mapper)
        {
            _repository = third_PartyRepository;
            _mapper = mapper;
        }


        public async Task<IEnumerable<Third_PartyDTO>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<Third_PartyDTO>>(entities);
        }


        public async Task<Third_PartyDTO> GetByIdAsync(string id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) throw new BusinessException("Persona Natural No Existe");
            return _mapper.Map<Third_PartyDTO>(entity);
        }

        public async Task<Third_PartyDTO> CreateAsync(Third_PartyCreateDTO dto)
        {
            var entity = _mapper.Map<Third_Party>(dto);
            entity.create_At = DateTime.UtcNow;
            entity.state = true;

            var created = await _repository.CreateAsync(entity);
            return _mapper.Map<Third_PartyDTO>(created);
        }

        public async Task<Third_PartyDTO> UpdateAsync(string id, Third_PartyUpdateDTO dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) throw new BusinessException("Natural_Person not found.");
            // Validamos consistencia
            if (dto.thirdParty_Id != id)
                throw new BusinessException("ID inconsistente entre ruta y entidad.");

            _mapper.Map(dto, entity);
            entity.update_At = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(entity);
            return _mapper.Map<Third_PartyDTO>(updated);
        }
        public async Task<bool> DeleteAsync(string id)
        {

            var result = await _repository.DeleteAsync(id);
            if (!result) throw new BusinessException("Tercero  not exist.");
            return result;
        }




    }
    
}
