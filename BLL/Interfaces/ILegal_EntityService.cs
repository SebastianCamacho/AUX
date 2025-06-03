using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ILegal_EntityService
    {
        Task<IEnumerable<Legal_EntityDTO>> GetAllAsync();
        Task<Legal_EntityDTO> GetByIdAsync(string id);
        Task<Legal_EntityDTO> CreateAsync(Legal_EntityCreateDTO dto);
        Task<Legal_EntityDTO> UpdateAsync(string id, Legal_EntityUpdateDTO dto);
        Task<bool> DeleteAsync(string id);
    }

}
