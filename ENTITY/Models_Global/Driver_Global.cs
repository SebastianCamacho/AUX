using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY.Models_Global
{
    public class Driver_Global
    {
        [Key] // Indica que 'DriverGlobalRecordId' es la clave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Generación automática de la clave primaria
        public int DriverGlobalRecordId { get; set; }

        [Required] // La cédula no puede ser nula
        public string driver_Global_Id { get; set; }

        [Required] // El TenantId (inquilino A) no puede ser nulo
        public string TenantId { get; set; }

        //Otras Propiedades
        public string type_Id { get; set; }
        public string? image { get; set; }
        public string first_Name { get; set; }
        public string first_Last_Name { get; set; }
        public string? second_Name { get; set; }
        public string? second_Last_Name { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public string? web_Site { get; set; }


        // Propiedad de navegación para Document_Driver_Global
        // Relación: Un Driver_Global puede tener muchos Document_Driver_Global
        public virtual ICollection<Document_Driver_Global> Document_Driver_Globals { get; set; }

        public Driver_Global()
        {
        }

        public Driver_Global(int driverGlobalRecordId, string driver_Global_Id, string tenantId, string type_Id, string? image, string first_Name, string first_Last_Name, string? second_Name, string? second_Last_Name, string? phone, string? email, string? web_Site, ICollection<Document_Driver_Global> document_Driver_Globals)
        {
            DriverGlobalRecordId = driverGlobalRecordId;
            this.driver_Global_Id = driver_Global_Id;
            TenantId = tenantId;
            this.type_Id = type_Id;
            this.image = image;
            this.first_Name = first_Name;
            this.first_Last_Name = first_Last_Name;
            this.second_Name = second_Name;
            this.second_Last_Name = second_Last_Name;
            this.phone = phone;
            this.email = email;
            this.web_Site = web_Site;
            Document_Driver_Globals = document_Driver_Globals;
        }
    }
}