using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class Administrador : Usuario
    {
        public Administrador()
        {
            
        }

        public Administrador(string identificacion, string? nombres, string? apellidos, string? foto, int idUser) : base(identificacion, nombres, apellidos, foto, idUser)
        {
        }
    }
}
