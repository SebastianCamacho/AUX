using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY.Models_Histories
{
    public class Driver_History
    {
        // --- Campos específicos del Historial ---
        [Key] // Clave Primaria de la tabla de historial
        public int HistoryId { get; set; } // Autoincremental
        [Required]
        public string DriverAuditId { get; set; } // Guarda el driver_Id (cédula) del Driver original
        [Required]
        public DateTime ActionTimestamp { get; set; } // Fecha y hora del cambio
        [Required]
        public string ActionType { get; set; }// Ej: "CREADO", "ACTUALIZADO", "ELIMINADO"
        [Required]
        public string ChangedByUserId { get; set; } // Quién hizo el cambio

        // --- Copia de los Campos de la Entidad Driver ---
        // Estos campos almacenan los valores que tenía el Driver en el momento del 'ActionTimestamp'.
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
        public string? email { get; set; }
        public string? web_Site { get; set; }

        public bool state { get; set; } // El estado que tenía el Driver en ese momento

        // Campos de auditoría del registro ORIGINAL de Driver (no del registro de historial)
        // Es útil saber cuándo se creó/modificó originalmente el registro Driver que generó esta entrada de historial.
        public DateTime create_At_OriginalDriver { get; set; }
        public DateTime update_At_OriginalDriver { get; set; }
        public string create_By_OriginalDriver { get; set; }
        public string update_By_OriginalDriver { get; set; }

        // PROPIEDAD DE NAVEGACIÓN ---
        public virtual ICollection<Document_Driver_History> SnapshottedDocuments { get; set; } = new List<Document_Driver_History>();

        public Driver_History()
        {
            
        }
    }
}
