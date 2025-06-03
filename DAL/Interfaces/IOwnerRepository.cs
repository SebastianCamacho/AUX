using ENTITY;
using ENTITY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IOwnerRepository
    {
        Task<IEnumerable<Owner>> GetAllAsync();
        Task<Owner> GetByIdAsync(string id);
        Task<Owner> CreateAsync(Owner entity);
        Task<Owner> UpdateAsync(Owner entity);
        Task<bool> DeleteAsync(string id, bool existinContracting);
    }
}
