using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IThird_PartyService
    {
        Task<IEnumerable<Third_PartyDTO>> GetAllAsync();
        Task<Third_PartyDTO> GetByIdAsync(string id);
        Task<Third_PartyDTO> CreateAsync(Third_PartyCreateDTO dto);
        Task<Third_PartyDTO> UpdateAsync(string id, Third_PartyUpdateDTO dto);
        Task<bool> DeleteAsync(string id);

    }
}
