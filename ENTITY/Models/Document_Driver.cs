using ENTITY.Models_Global;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ENTITY.Models
{
    public class Document_Driver
    {
        [Key]
        public string id_Document { get; set; }
        public string type_Document { get; set; }
        public DateTime start_validity { get; set; }
        public DateTime? end_validity { get; set; }
        public string image_Soport { get; set; }
        public bool is_Expirable { get; set; }

        // Clave Foránea para Driver_Global
        // Relación: Un Document_Driver_Global pertenece a un Driver_Global
        [Required]
        public string driver_Id { get; set; } // FK

        [ForeignKey("driver_Id")] // Especifica que la propiedad 'Driver_Global' usa 'driver_Id' como FK
        public Driver Driver { get; set; }
        public Document_Driver()
        {
            
        }
        public Document_Driver(string id_Document, string type_Document, DateTime start_validity, DateTime? end_validity, string image_Soport, bool is_Expirable, string driver_Id, Driver driver)
        {
            this.id_Document = id_Document;
            this.type_Document = type_Document;
            this.start_validity = start_validity;
            this.end_validity = end_validity;
            this.image_Soport = image_Soport;
            this.is_Expirable = is_Expirable;
            this.driver_Id = driver_Id;
            Driver = driver;
        }
    }
}