using ENTITY.Models_Histories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces_Histories
{
    public interface IDocumentDriverHistoryRepository
    {
        /// <summary>
        /// Añade un nuevo registro de historial de documento.
        /// El guardado final en BD (SaveChangesAsync) se espera que lo maneje el servicio.
        /// </summary>
        /// <param name="documentHistory">La entidad de historial de documento a añadir.</param>
        /// <returns>La entidad de historial de documento añadida al contexto.</returns>
        Task<Document_Driver_History> AddAsync(Document_Driver_History documentHistory);

        /// <summary>
        /// Añade una colección de registros de historial de documentos.
        /// El guardado final en BD (SaveChangesAsync) se espera que lo maneje el servicio.
        /// </summary>
        /// <param name="documentHistories">La colección de entidades de historial a añadir.</param>
        Task AddRangeAsync(IEnumerable<Document_Driver_History> documentHistories);
    }
}
