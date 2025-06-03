using DAL.Interfaces;
using ENTITY;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class Contracting_PartyRepository : IContracting_PartyRepository
    {
        private readonly FUEC_DbContext _context;
        public Contracting_PartyRepository(FUEC_DbContext context)
        {
            _context = context;
        }
        // Implementar métodos de IContracting_PartyRepository aquí
        public async Task<IEnumerable<Contracting_Party>> GetAllAsync()
        {
            return await _context.Contracting_Partys
                .Include(x => x.Third_Party)
                .ThenInclude(x => x.Address)
                .ToListAsync();
        }
        public async Task<Contracting_Party> GetByIdAsync(string id)
        {
            return await _context.Contracting_Partys
                .Include(x => x.Third_Party)
                .ThenInclude(x => x.Address)
                .FirstOrDefaultAsync(x => x.third_Id == id);
        }
        public async Task<Contracting_Party> CreateAsync(Contracting_Party entity)
        {
            _context.Contracting_Partys.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<Contracting_Party> UpdateAsync(Contracting_Party entity)
        {
            _context.Contracting_Partys.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<bool> DeleteAsync(string id, bool existingOwner)
        {
            
            var entity = await _context.Contracting_Partys.Include(x => x.Third_Party).FirstOrDefaultAsync(x => x.third_Id == id);
            if (entity == null) return false;
            _context.Contracting_Partys.Remove(entity);

            if (!existingOwner) //Si existe contratante con el mismo id solo se elimina el owner y no el third
            {
                _context.Third_Party.Remove(entity.Third_Party);
            }

            await _context.SaveChangesAsync();
            return true;





        }
    }
}
