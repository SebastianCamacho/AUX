using ENTITY.Models_Histories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces_Histories
{
    public interface IDriverHistoryRepository
    {
        /// <summary>
        /// Añade un nuevo registro de historial de driver.
        /// </summary>
        /// <param name="driverHistory">La entidad de historial a añadir.</param>
        /// <returns>La entidad de historial añadida.</returns>
        Task<Driver_History> AddAsync(Driver_History driverHistory);
    }
}
