using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class DriverDTO // Para mostrar la info combinada de un conductor (vista del cliente)
    {
        // --- Identificador Principal (el que conoce el cliente) ---
        public string driver_Id { get; set; } // La cédula

        // --- Datos Personales ---
        public string type_Id { get; set; }
        public string? image { get; set; }
        public string first_Name { get; set; }
        public string first_Last_Name { get; set; }
        public string? second_Name { get; set; }
        public string? second_Last_Name { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public string? web_Site { get; set; }
        public bool state { get; set; }

        // --- Auditoría (si es relevante para el cliente) ---
        public DateTime create_At { get; set; }
        public DateTime update_At { get; set; }
        public string create_By { get; set; }
        public string update_By { get; set; }
        public List<DocumentDTO> Documents { get; set; } = new List<DocumentDTO>();

    }

    public class DriverCreateDTO // Para crear un nuevo conductor
    {
        [Required]
        public string driver_Id { get; set; } // La Cédula

        [Required]
        public string type_Id { get; set; }

        public string? image { get; set; }

        [Required]
        public string first_Name { get; set; }
        [Required]
        public string first_Last_Name { get; set; }
        public string? second_Name { get; set; }
        public string? second_Last_Name { get; set; }
        public string? phone { get; set; }
        [EmailAddress]
        public string? email { get; set; }
        public string? web_Site { get; set; }

        

        public List<DocumentCreateDTO>? Documents { get; set; } = new List<DocumentCreateDTO>();
    }
    public class DriverUpdateDTO // Para actualizar un conductor existente
    {
        // No se incluye driver_Id aquí, se pasa como parámetro
        public string? type_Id { get; set; }
        public string? image { get; set; }
        public string? first_Name { get; set; }
        public string? first_Last_Name { get; set; }
        public string? second_Name { get; set; }
        public string? second_Last_Name { get; set; }
        public string? phone { get; set; }
        [EmailAddress]
        public string? email { get; set; }
        public string? web_Site { get; set; }
        public bool? state { get; set; } // Nullable para indicar si se quiere actualizar o no
    }



}
