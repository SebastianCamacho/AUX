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
    public class AddressRepository : IAddressRepository
    {
        private readonly FUEC_DbContext _context;

        public AddressRepository(FUEC_DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Address>> GetAllAsync()
        {
            return await _context.Address.ToListAsync();
        }

        public async Task<Address> GetByIdAsync(int id)
        {
            return await _context.Address.FindAsync(id);
        }

        public async Task AddAsync(Address entity)
        {
            _context.Address.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Address entity)
        {
            _context.Address.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Address.FindAsync(id);
            if (entity != null)
            {
                _context.Address.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
