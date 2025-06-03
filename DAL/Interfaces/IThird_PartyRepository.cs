using ENTITY;
using ENTITY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IThird_PartyRepository
    {
        Task<IEnumerable<Third_Party>> GetAllAsync();
        Task<Third_Party> GetByIdAsync(string id);
        Task<Third_Party> CreateAsync(Third_Party entity);
        Task<Third_Party> UpdateAsync(Third_Party entity);
        Task<bool> DeleteAsync(string id);



    }
}
