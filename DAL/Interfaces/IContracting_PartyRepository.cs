using ENTITY;
using ENTITY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IContracting_PartyRepository
    {
            Task<IEnumerable<Contracting_Party>> GetAllAsync();
            Task<Contracting_Party> GetByIdAsync(string id);
            Task<Contracting_Party> CreateAsync(Contracting_Party entity);
            Task<Contracting_Party> UpdateAsync(Contracting_Party entity);
            Task<bool> DeleteAsync(string id, bool existingOwner);
    }
}
