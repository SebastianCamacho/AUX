using AutoMapper;
using BLL.DTOs;
using BLL.Exceptions;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Repositories;
using ENTITY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IOwnerRepository _repo;
        private readonly IMapper _mapper;

        public OwnerService(IOwnerRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OwnerDTO>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<OwnerDTO>>(list);
        }

        public async Task<OwnerDTO> GetByIdAsync(string id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return _mapper.Map<OwnerDTO>(entity);
        }

        public async Task AddAsync(OwnerDTO dto)
        {
            var owner = _mapper.Map<Owner>(dto);

            if (dto.ThirdParty is NaturalPersonDTO np)
                owner.Third_Party = _mapper.Map<Natural_Person>(np);
            else if (dto.ThirdParty is LegalEntityDTO le)
                owner.Third_Party = _mapper.Map<Legal_Entity>(le);

            await _repo.AddAsync(owner);
        }

        public async Task UpdateAsync(string id, OwnerDTO dto)
        {
            var existingOwner = await _repo.GetByIdAsync(id);

            if (existingOwner == null)
                throw new BusinessException($"El Owner (id:{id}) que intenta actualizar no existe en la base de datos ");

            // Mapear los datos del DTO al Owner existente
            _mapper.Map(dto, existingOwner);

            try
            {
                await _repo.UpdateAsync(existingOwner);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al actualizar el Owner con ID {id}: {ex.Message}");
            }
        }
        
        public async Task DeleteAsync(string id)
        {
            //agregar funcionalidad con el id del third 
                var owner = await _repo.GetByIdAsync(id);
                if (owner == null)
                    throw new BusinessException($"No se encontró ningún Owner con el ID '{id}'.");

                await _repo.DeleteAsync(id);
        }
    }
}
