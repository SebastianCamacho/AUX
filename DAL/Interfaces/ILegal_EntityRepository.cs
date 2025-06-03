using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    using ENTITY.Models;

    public interface ILegal_EntityRepository
    {
        Task<IEnumerable<Legal_Entity>> GetAllAsync();
        Task<Legal_Entity> GetByIdAsync(string id);
        Task<Legal_Entity> CreateAsync(Legal_Entity entity);
        Task<Legal_Entity> UpdateAsync(Legal_Entity entity);
        Task<bool> DeleteAsync(string id);
    }

}
