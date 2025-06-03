using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY.Models_Histories
{
    public class Document_Driver_History
    {
        // --- Campos específicos del Historial de Documento ---
        public int DocHistoryId { get; set; }          // PK de esta tabla, se configura en Fluent API como autoincremental
        [Required]
        public string DocumentAuditId { get; set; }     // Guarda el 'id_Document' (string) del Document_Driver original
        [Required]
        public DateTime ActionTimestamp { get; set; }   // Fecha y hora de esta "foto"
        [Required]
        public string ActionType { get; set; }          // "CREADO", "ACTUALIZADO", "ELIMINADO", "SNAPSHOT"
        [Required]
        public string ChangedByUserId { get; set; }     // Quién hizo el cambio que originó esta foto

        // --- Enlace CLAVE al registro de Driver_History padre ---
        public int DriverHistoryId_FK { get; set; }     // FK que apunta a Driver_History.HistoryId
        public virtual Driver_History DriverHistory { get; set; } // Define la relación


        // --- Copia de los Campos de la Entidad Document_Driver ---
        public string type_Document { get; set; }
        public DateTime start_validity { get; set; }
        public DateTime? end_validity { get; set; }
        public string? image_Soport { get; set; }      // Ya era nullable
        public bool is_Expirable { get; set; }

        // El 'driver_Id' (cédula del conductor) original, para referencia y consistencia.
        public string driver_Id_Original { get; set; }

    }
}
