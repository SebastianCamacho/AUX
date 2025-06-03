using DAL.Interfaces;
using ENTITY;
using ENTITY.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class Third_PartyRepository : IThird_PartyRepository
    {
        private readonly FUEC_DbContext _context;

        public Third_PartyRepository(FUEC_DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Third_Party>> GetAllAsync() 
        {
            return await _context.Third_Party.ToListAsync(); 
        }


        public async Task<Third_Party> GetByIdAsync(string id)
        {
            return await _context.Third_Party
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.thirdParty_Id == id);
        }

        public async Task<Third_Party> CreateAsync(Third_Party entity)
        {
            _context.Third_Party.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Third_Party> UpdateAsync(Third_Party entity)
        {
            _context.Third_Party.Update(entity); // Como es TPH, EF actualiza correctamente según tipo
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _context.Third_Party.FindAsync(id);
            if (entity == null) return false;

            _context.Third_Party.Remove(entity);
            await _context.SaveChangesAsync();
            return true; 
        }
        
    }
}
