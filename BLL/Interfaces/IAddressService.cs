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
        Task<IEnumerable<AddressDTO>> GetAllAsync();
        Task<AddressDTO> GetByIdAsync(int id);
        Task<AddressDTO> CreateAsync(AddressCreateDTO dto);
        Task<AddressDTO> UpdateAsync(int id, AddressUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
        
        
    }
}
