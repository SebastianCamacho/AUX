using BLL.Exceptions;
using BLL.Interfaces;
using DAL.Interfaces;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTOs;

namespace BLL.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;

        public AddressService(IAddressRepository addressRepository, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        public async Task<AddressDTO> CreateAsync(AddressCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Third_Id))
                throw new BusinessException("El id del Tercero es obligatorio.");

            var entity = _mapper.Map<Address>(dto);
            await _addressRepository.AddAsync(entity);
            return _mapper.Map<AddressDTO>(entity);
        }

        public async Task<AddressDTO> UpdateAsync(int id, AddressCreateDTO dto)
        {
            var existing = await _addressRepository.GetByIdAsync(id);
            if (existing == null)
                throw new BusinessException($"No se encontró una dirección con ID {id}.");

            _mapper.Map(dto, existing); // Mapear valores sobre la entidad existente
            await _addressRepository.UpdateAsync(existing);
            return _mapper.Map<AddressDTO>(existing);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _addressRepository.GetByIdAsync(id);
            if (existing == null)
                throw new BusinessException($"No se puede eliminar: dirección con ID {id} no existe.");

            await _addressRepository.DeleteAsync(id);
        }

        public async Task<AddressDTO> GetByIdAsync(int id)
        {
            var entity = await _addressRepository.GetByIdAsync(id);
            if (entity == null)
                throw new BusinessException($"No se encontró dirección con ID {id}.");

            return _mapper.Map<AddressDTO>(entity);
        }

        public async Task<List<AddressDTO>> GetAllAsync()
        {
            var list = await _addressRepository.GetAllAsync();
            return _mapper.Map<List<AddressDTO>>(list);
        }
    }

}
