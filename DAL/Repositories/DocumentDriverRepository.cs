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
    public class DocumentDriverRepository : IDocumentDriverRepository
    {
        private readonly FUEC_DbContext _context;

        public DocumentDriverRepository(FUEC_DbContext context)
        {
            _context = context;
        }

        public async Task<Document_Driver?> GetByIdAsync(string idDocument)
        {
            return await _context.Document_Drivers.FindAsync(idDocument);
        }

        public async Task<IEnumerable<Document_Driver>> GetByDriverIdAsync(string driverId)
        {
            return await _context.Document_Drivers
                                 .Where(doc => doc.driver_Id == driverId)
                                 .ToListAsync();
        }
        // Método para GetDriversByTenantAsync optimizado
        public async Task<IEnumerable<Document_Driver>> GetByDriverIdsAsync(IEnumerable<string> driverIds)
        {
            if (driverIds == null || !driverIds.Any())
            {
                return Enumerable.Empty<Document_Driver>();
            }
            return await _context.Document_Drivers
                                 .Where(d => driverIds.Contains(d.driver_Id))
                                 .ToListAsync();
        }

        public async Task<Document_Driver> AddAsync(Document_Driver entity)
        {
            await _context.Document_Drivers.AddAsync(entity); // <-- Solo añade al contexto
                                                              // await _context.SaveChangesAsync(); // <-- ELIMINADO
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<Document_Driver> entities) // Si tienes este método
        {
            await _context.Document_Drivers.AddRangeAsync(entities); // <-- Solo añade al contexto
        }

        public Task<Document_Driver> UpdateAsync(Document_Driver entity)
        {
            _context.Entry(entity).State = EntityState.Modified; // <-- Solo marca como modificado
                                                                 // await _context.SaveChangesAsync(); // <-- ELIMINADO
            return Task.FromResult(entity);
        }

        public async Task<bool> DeleteAsync(string idDocument)
        {
            var entity = await _context.Document_Drivers.FindAsync(idDocument);
            if (entity == null)
                return false;

            _context.Document_Drivers.Remove(entity); // <-- Solo marca para eliminar
                                                      // await _context.SaveChangesAsync(); // <-- ELIMINADO
            return true;
        }
    }
}
