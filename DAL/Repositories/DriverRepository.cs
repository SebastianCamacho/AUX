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
    
    public class DriverRepository : IDriverRepository
    {
        private readonly FUEC_DbContext _context;

        public DriverRepository(FUEC_DbContext context)
        {
            _context = context;
        }

        public async Task<Driver?> GetByIdAsync(string driverId)
        {
            return await _context.Drivers
                                    .Include(d => d.Document_Drivers)
                                    .FirstOrDefaultAsync(d => d.driver_Id == driverId);
        }

        public async Task<IEnumerable<Driver>> GetAllAsync()
        {
            return await _context.Drivers
                                    .Include(d => d.Document_Drivers)
                                    .ToListAsync();
        }

        public async Task<Driver> AddAsync(Driver driver)
        {
            await _context.Drivers.AddAsync(driver); // <-- Solo añade al contexto
                                                        // await _context.SaveChangesAsync(); // <-- ELIMINADO
            return driver;
        }

        public Task<Driver> UpdateAsync(Driver driver) // Puede no ser async si solo cambia estado
        {
            _context.Entry(driver).State = EntityState.Modified; // <-- Solo marca como modificado
                                                                    // await _context.SaveChangesAsync(); // <-- ELIMINADO
            return Task.FromResult(driver); // Devolvemos la entidad en una Tarea completada
        }

        public async Task<bool> DeleteAsync(string driverId)
        {
            var driverToDelete = await _context.Drivers.FindAsync(driverId);
            if (driverToDelete == null)
            {
                return false;
            }
            _context.Drivers.Remove(driverToDelete); // <-- Solo marca para eliminar
                                                        // await _context.SaveChangesAsync(); // <-- ELIMINADO
            return true;
        }
    }
}
