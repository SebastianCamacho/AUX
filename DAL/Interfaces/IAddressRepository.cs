using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IAddressRepository
    {
        Task<IEnumerable<Address>> GetAllAsync(); // Obtiene todas las direcciones
        Task<Address> GetByIdAsync(int id); // Obtiene una dirección por su ID
        Task AddAsync(Address entity); // Agrega una nueva dirección
        Task UpdateAsync(Address entity); // Actualiza una dirección existente
        Task DeleteAsync(int id); // Elimina una dirección por su ID
    }
}
