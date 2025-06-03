using AutoMapper;
using BLL.DTOs;
using BLL.Exceptions;
using BLL.Interfaces;
using DAL.Interfaces;
using ENTITY;
using ENTITY.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class OwnerService : IOwnerService
    {

        // Aquí puedes inyectar tus dependencias, como repositorios o servicios
        private readonly IOwnerRepository _repository;
        private readonly IThird_PartyRepository _repositoryThird_Party;
        private readonly IContracting_PartyRepository _repositoryContracting_Party;
        private readonly IMapper _mapper;
        public OwnerService(IOwnerRepository ownerRepository, IMapper mapper, IThird_PartyRepository repositoryThird_Party, IContracting_PartyRepository repositoryContracting_Party)
        {
            _repository = ownerRepository;
            _mapper = mapper;
            _repositoryThird_Party = repositoryThird_Party;
            _repositoryContracting_Party = repositoryContracting_Party;

        }
        // Implementa los métodos de IOwnerService aquí

        public async Task<IEnumerable<OwnerDTO>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<OwnerDTO>>(entities);
        }
        public async Task<OwnerDTO> GetByIdAsync(string id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) throw new BusinessException("Propietario No Existe");
            return _mapper.Map<OwnerDTO>(entity);
        }
        public async Task<OwnerDTO> CreateAsync(OwnerCreateDTO dto)
        {
            var existingThird = await _repositoryThird_Party.GetByIdAsync(dto.Third_Party.thirdParty_Id);

            var entity = _mapper.Map<Owner>(dto);


            if (existingThird != null)
            {
                entity.Third_Party = existingThird; // Esto evita el conflicto de tracking
                return _mapper.Map<OwnerDTO>(await _repository.CreateAsync(entity));
            }
            
            entity.Third_Party.create_At = DateTime.UtcNow;
            entity.Third_Party.state = true;
            return _mapper.Map<OwnerDTO>(await _repository.CreateAsync(entity));


        }

        public async Task<OwnerDTO> UpdateAsync(string id, OwnerUpdateDTO dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) throw new BusinessException("Propietario No existe.");
            // Validamos consistencia
            if (dto.Third_Party.thirdParty_Id != id)
                throw new BusinessException("ID inconsistente entre ruta y entidad.");

            _mapper.Map(dto, entity);
            entity.Third_Party.update_At = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(entity);
            return _mapper.Map<OwnerDTO>(updated);
        }
        public async Task<bool> DeleteAsync(string id)
        {
            bool result;
            var existingContracting = await _repositoryContracting_Party.GetByIdAsync(id);

            if (existingContracting != null)
            {
                result = await _repository.DeleteAsync(id, true);
            }
            else
            {
                result = await _repository.DeleteAsync(id, false);
            }

            if (!result) throw new BusinessException("Propietario  no Exist.");

            return result;

        }

    }
    



}
