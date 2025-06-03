using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ENTITY.Models_Global
{
    public class Document_Driver_Global
    {
        [Key]
        public string id_Document { get; set; }
        public string type_Document { get; set; }
        public DateTime start_validity { get; set; }
        public DateTime? end_validity { get; set; }
        public string? image_Soport { get; set; }
        public bool is_Expirable { get; set; }

        // Clave Foránea para Driver_Global
        // Relación: Un Document_Driver_Global pertenece a un Driver_Global
        [Required] // La FK al Driver_Global no puede ser nula
        public int DriverGlobalRecordId { get; set; }

        [ForeignKey("DriverGlobalRecordId")] // Especifica que la propiedad 'Driver_Global' usa 'driver_Global_Id' como FK
        public Driver_Global Driver_Global { get; set; }

        public Document_Driver_Global()
        {

        }

        public Document_Driver_Global(string id_Document, string type_Document, DateTime start_validity, DateTime? end_validity, string? image_Soport, bool is_Expirable, int driverGlobalRecordId, Driver_Global driver_Global)
        {
            this.id_Document = id_Document;
            this.type_Document = type_Document;
            this.start_validity = start_validity;
            this.end_validity = end_validity;
            this.image_Soport = image_Soport;
            this.is_Expirable = is_Expirable;
            DriverGlobalRecordId = driverGlobalRecordId;
            Driver_Global = driver_Global;
        }
    }
}