using AutoMapper;
using BLL.DTOs;
using BLL.Exceptions;
using BLL.Interfaces;
using DAL.Interfaces;
using ENTITY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class Legal_EntityService : ILegal_EntityService
    {
        private readonly ILegal_EntityRepository _repository;
        private readonly IMapper _mapper;

        public Legal_EntityService(ILegal_EntityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Legal_EntityDTO>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<Legal_EntityDTO>>(entities);
        }

        public async Task<Legal_EntityDTO> GetByIdAsync(string id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new BusinessException("Persona Juridica No Existe");

            return _mapper.Map<Legal_EntityDTO>(entity);
        }

        public async Task<Legal_EntityDTO> CreateAsync(Legal_EntityCreateDTO dto)
        {
            var entity = _mapper.Map<Legal_Entity>(dto);
            entity.create_At = DateTime.UtcNow;
            entity.state = true;

            var createdEntity = await _repository.CreateAsync(entity);
            return _mapper.Map<Legal_EntityDTO>(createdEntity);
        }

        public async Task<Legal_EntityDTO> UpdateAsync(string id, Legal_EntityUpdateDTO dto)
        {
            var existingEntity = await _repository.GetByIdAsync(id);
            if (existingEntity == null)
                throw new BusinessException("Entidad no encontrada");

            _mapper.Map(dto, existingEntity);
            existingEntity.update_At = DateTime.UtcNow;
            var updatedEntity = await _repository.UpdateAsync(existingEntity);
            return _mapper.Map<Legal_EntityDTO>(updatedEntity);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _repository.DeleteAsync(id);
            if (!result)
                throw new BusinessException("Entidad no encontrada");

            return result;
        }
    }

}
