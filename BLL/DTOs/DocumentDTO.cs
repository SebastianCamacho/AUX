using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BLL.DTOs
{
// --- DTOs para Documentos ---

public class DocumentDTO // Para mostrar info de un documento
    {
        public string id_Document { get; set; } // El ID único del documento
        public string type_Document { get; set; }
        public DateTime start_validity { get; set; }
        public DateTime? end_validity { get; set; }
        public string? image_Soport { get; set; }
        public bool is_Expirable { get; set; }
    }

    public class DocumentCreateDTO // Para crear un nuevo documento
    {
        [Required]
        public string id_Document { get; set; }

        [Required]
        public string type_Document { get; set; }

        [Required]
        public DateTime start_validity { get; set; }

        public DateTime? end_validity { get; set; }

        public string? image_Soport { get; set; }

        public bool is_Expirable { get; set; }
    }
    public class DocumentUpdateDTO
    {
        // El id_Document se pasará como parámetro en la ruta, no aquí.
        public string? type_Document { get; set; }
        public DateTime? start_validity { get; set; }
        public DateTime? end_validity { get; set; }
        public string? image_Soport { get; set; } // URL o base64
        public bool? is_Expirable { get; set; }
    }
}
