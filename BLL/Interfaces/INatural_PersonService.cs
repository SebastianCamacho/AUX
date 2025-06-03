using AutoMapper;
using BLL.DTOs;
using BLL.Exceptions;
using DAL.Interfaces;
using ENTITY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface INatural_PersonService
    {
        Task<IEnumerable<Natural_PersonDTO>> GetAllAsync();
        Task<Natural_PersonDTO> GetByIdAsync(string id);
        Task<Natural_PersonDTO> CreateAsync(Natural_PersonCreateDTO dto);
        Task<Natural_PersonDTO> UpdateAsync(string id, Natural_PersonUpdateDTO dto);
        Task<bool> DeleteAsync(string id);
    }


}
