using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IContracting_PartyService
    {
        Task<IEnumerable<Contracting_PartyDTO>> GetAllAsync();
        Task<Contracting_PartyDTO> GetByIdAsync(string id);
        Task<Contracting_PartyDTO> CreateAsync(Contracting_PartyCreateDTO dto);
        Task<Contracting_PartyDTO> UpdateAsync(string id, Contracting_PartyUpdateDTO dto);
        Task<bool> DeleteAsync(string id);
    }
}
