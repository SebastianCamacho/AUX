using DAL.Interfaces;
using ENTITY.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly FUEC_DbContext _context;
        public OwnerRepository(FUEC_DbContext context)
        {
            _context = context;
        }
        // Implementar métodos de IOwnerRepository aquí
        public async Task<IEnumerable<Owner>> GetAllAsync()
        {

            return await _context.Owners
                .Include(x => x.Third_Party)
                .ThenInclude(x => x.Address)
                .ToListAsync();
        }
        public async Task<Owner> GetByIdAsync(string id)
        {
            return await _context.Owners
                .Include(x => x.Third_Party)
                .ThenInclude(x => x.Address)
                .FirstOrDefaultAsync(x => x.third_Id == id);
        }
        public async Task<Owner> CreateAsync(Owner entity)
        {
            _context.Owners.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<Owner> UpdateAsync(Owner entity)
        {
            _context.Owners.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<bool> DeleteAsync(string id, bool existinContracting)
        {
            var entity = await _context.Owners.Include(x => x.Third_Party).FirstOrDefaultAsync(x => x.third_Id == id);
            if (entity == null) return false;
            _context.Owners.Remove(entity);

            if (!existinContracting) //Si existe contratante con el mismo id solo se elimina el owner y no el third
            {
                _context.Third_Party.Remove(entity.Third_Party);
            }

            await _context.SaveChangesAsync();
            return true;
        }


    }
   
}
