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
    public class Natural_PersonRepository : INatural_PersonRepository
    {
        private readonly FUEC_DbContext _context;

        public Natural_PersonRepository(FUEC_DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Natural_Person>> GetAllAsync()
        {
            return await _context.Natural_Persons.ToListAsync();
        }

        public async Task<Natural_Person> GetByIdAsync(string id)
        {
            return await _context.Natural_Persons.FindAsync(id);
        }

        public async Task<Natural_Person> CreateAsync(Natural_Person entity)
        {
            _context.Natural_Persons.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Natural_Person> UpdateAsync(Natural_Person entity)
        {
            _context.Natural_Persons.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _context.Natural_Persons.FindAsync(id);
            if (entity == null) return false;

            _context.Natural_Persons.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
