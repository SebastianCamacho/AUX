using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IAddressService
    {
        Task<AddressDTO> CreateAsync(AddressCreateDTO dto);
        Task<AddressDTO> UpdateAsync(int id, AddressCreateDTO dto);
        Task DeleteAsync(int id);
        Task<AddressDTO> GetByIdAsync(int id);
        Task<List<AddressDTO>> GetAllAsync();
    }
}
