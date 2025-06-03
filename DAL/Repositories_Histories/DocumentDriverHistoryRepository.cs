using DAL.Interfaces_Histories;
using ENTITY.Models_Histories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories_Histories
{
    public class DocumentDriverHistoryRepository : IDocumentDriverHistoryRepository
    {
        private readonly FUEC_DbContext _context;

        public DocumentDriverHistoryRepository(FUEC_DbContext context)
        {
            _context = context;
        }

        public async Task<Document_Driver_History> AddAsync(Document_Driver_History documentHistory)
        {
            await _context.Document_Driver_Histories.AddAsync(documentHistory); // <-- Solo añade al contexto
                                                                                // await _context.SaveChangesAsync(); // <-- ELIMINADO
            return documentHistory;
        }

        public async Task AddRangeAsync(IEnumerable<Document_Driver_History> documentHistories)
        {
            await _context.Document_Driver_Histories.AddRangeAsync(documentHistories); // <-- Solo añade al contexto
                                                                                       // await _context.SaveChangesAsync(); // <-- ELIMINADO
        }
    }
}
