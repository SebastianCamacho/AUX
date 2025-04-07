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
        Task AddAsync(OwnerDTO dto);
        Task UpdateAsync(string id, OwnerDTO dto);
        Task DeleteAsync(string id);
    }
}
