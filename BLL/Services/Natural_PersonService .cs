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
    public class Natural_PersonService : INatural_PersonService
    {
        private readonly INatural_PersonRepository _repository;
        private readonly IMapper _mapper;

        public Natural_PersonService(INatural_PersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Natural_PersonDTO>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<Natural_PersonDTO>>(list);
        }

        public async Task<Natural_PersonDTO> GetByIdAsync(string id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) throw new BusinessException("Persona Natural No Existe");
            return _mapper.Map<Natural_PersonDTO>(entity);
        }

        public async Task<Natural_PersonDTO> CreateAsync(Natural_PersonCreateDTO dto)
        {
            var entity = _mapper.Map<Natural_Person>(dto);
            entity.create_At = DateTime.UtcNow;
            entity.state = true;

            var created = await _repository.CreateAsync(entity);
            return _mapper.Map<Natural_PersonDTO>(created);
        }

        public async Task<Natural_PersonDTO> UpdateAsync(string id, Natural_PersonUpdateDTO dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) throw new BusinessException("Natural_Person not found.");
            var auxDTO = _mapper.Map<Natural_Person>(dto);
            _mapper.Map(auxDTO, entity);
            entity.update_At = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(entity);
            return _mapper.Map<Natural_PersonDTO>(updated);
        }

        public async Task<bool> DeleteAsync(string id)
        {

            var result = await _repository.DeleteAsync(id);
            if (!result) throw new BusinessException("Natural_Person not exist.");
            return result;
        }
    }

}
