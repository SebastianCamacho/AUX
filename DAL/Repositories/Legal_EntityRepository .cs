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
    public class Legal_EntityRepository : ILegal_EntityRepository
    {
        private readonly FUEC_DbContext _context;

        public Legal_EntityRepository(FUEC_DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Legal_Entity>> GetAllAsync()
        {
            return await _context.Legal_Entities
                .Include(x => x.Address)
                .ToListAsync();
        }

        public async Task<Legal_Entity> GetByIdAsync(string id)
        {
            return await _context.Legal_Entities
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.thirdParty_Id == id);
        }

        public async Task<Legal_Entity> CreateAsync(Legal_Entity entity)
        {
            _context.Legal_Entities.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Legal_Entity> UpdateAsync(Legal_Entity entity)
        {
            _context.Legal_Entities.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _context.Legal_Entities.FindAsync(id);
            if (entity == null)
                return false;

            _context.Legal_Entities.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
