using DAL.Interfaces_Histories;
using ENTITY.Models_Histories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories_Histories
{
    public class DriverHistoryRepository : IDriverHistoryRepository
    {
        private readonly FUEC_DbContext _context;

        public DriverHistoryRepository(FUEC_DbContext context)
        {
            _context = context;
        }

        public async Task<Driver_History> AddAsync(Driver_History driverHistory)
        {
            await _context.Driver_Histories.AddAsync(driverHistory); // <-- Solo añade al contexto
                                                                     // await _context.SaveChangesAsync(); // <-- ELIMINADO
            return driverHistory;
        }
    }
}
