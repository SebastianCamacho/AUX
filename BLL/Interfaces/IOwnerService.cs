using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IOwnerService
    {
        Task<IEnumerable<OwnerDTO>> GetAllAsync();
        Task<OwnerDTO> GetByIdAsync(string id);
        Task<OwnerDTO> CreateAsync(OwnerCreateDTO dto);
        Task<OwnerDTO> UpdateAsync(string id, OwnerUpdateDTO dto);
        Task<bool> DeleteAsync(string id);
    }
}
