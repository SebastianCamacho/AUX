using AutoMapper;
using BLL.DTOs;
using BLL.Exceptions;
using BLL.Interfaces;
using DAL.Interfaces;
using ENTITY;
using ENTITY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class Contracting_PartyService : IContracting_PartyService
    {
        private readonly IContracting_PartyRepository _repository;
        private readonly IThird_PartyRepository _repositoryThird_Party;
        private readonly IOwnerRepository _repositoryOwner;
        private readonly IMapper _mapper;
        public Contracting_PartyService(IContracting_PartyRepository contracting_PartyRepository, IMapper mapper, IThird_PartyRepository repositoryThird_Party, IOwnerRepository repositoryOwner)
        {
            _repository = contracting_PartyRepository;
            _mapper = mapper;
            _repositoryThird_Party = repositoryThird_Party;
            _repositoryOwner = repositoryOwner;
        }

        public async Task<IEnumerable<Contracting_PartyDTO>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<Contracting_PartyDTO>>(entities);
        }

        public async Task<Contracting_PartyDTO> GetByIdAsync(string id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) throw new BusinessException("Contratante No Existe");
            return _mapper.Map<Contracting_PartyDTO>(entity);
        }

        public async Task<Contracting_PartyDTO> CreateAsync(Contracting_PartyCreateDTO dto)
        {
            var existingThird = await _repositoryThird_Party.GetByIdAsync(dto.Third_Party.thirdParty_Id);

            var entity = _mapper.Map<Contracting_Party>(dto);


            if (existingThird != null)
            {
                entity.Third_Party = existingThird; // Esto evita el conflicto de tracking
                return _mapper.Map<Contracting_PartyDTO>(await _repository.CreateAsync(entity));
            }

            entity.Third_Party.create_At = DateTime.UtcNow;
            entity.Third_Party.state = true;
            return _mapper.Map<Contracting_PartyDTO>(await _repository.CreateAsync(entity));

           
        }

        public async Task<Contracting_PartyDTO> UpdateAsync(string id, Contracting_PartyUpdateDTO dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) throw new BusinessException("Propietario not found.");
            // Validamos consistencia
            if (dto.Third_Party.thirdParty_Id != id)
                throw new BusinessException("ID inconsistente entre ruta y entidad.");

            _mapper.Map(dto, entity);
            entity.Third_Party.update_At = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(entity);
            return _mapper.Map<Contracting_PartyDTO>(updated);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            bool result;
            var existingOwner = await _repositoryOwner.GetByIdAsync(id);

            if (existingOwner != null)
            {
                result = await _repository.DeleteAsync(id, true);
            }
            else 
            {
                result = await _repository.DeleteAsync(id, false);
            }

            if (!result) throw new BusinessException("Contratante  no Exist.");

            return result;
            
        }
    }
}
