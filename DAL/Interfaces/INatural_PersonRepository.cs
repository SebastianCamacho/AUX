using ENTITY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface INatural_PersonRepository
    {
        Task<IEnumerable<Natural_Person>> GetAllAsync();
        Task<Natural_Person> GetByIdAsync(string id);
        Task<Natural_Person> CreateAsync(Natural_Person entity);
        Task<Natural_Person> UpdateAsync(Natural_Person entity);
        Task<bool> DeleteAsync(string id);
    }

}
