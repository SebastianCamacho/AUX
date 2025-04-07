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

        public async Task<IEnumerable<Owner>> GetAllAsync()
        {
            return await _context.Owners.Include(x => x.Third_Party).ThenInclude(x => x.Address).ToListAsync();
        }

        public async Task<Owner> GetByIdAsync(string id)
        {
            return await _context.Owners.Include(x => x.Third_Party).ThenInclude(x => x.Address)
                .FirstOrDefaultAsync(x => x.Third_Party.thirdParty_Id == id);
        }

        public async Task AddAsync(Owner owner)
        {
            await _context.Owners.AddAsync(owner);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Owner entity)
        {
            _context.Owners.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var owner = await _context.Owners.Include(x => x.Third_Party)
                .FirstOrDefaultAsync(x => x.Third_Party.thirdParty_Id == id);
            if (owner != null)
            {
                _context.Owners.Remove(owner);
                await _context.SaveChangesAsync();
            }
        }
    }
}
