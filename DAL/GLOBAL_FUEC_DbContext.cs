using ENTITY.Models_Global;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{

    public class GLOBAL_FUEC_DbContext : DbContext
    {
        public GLOBAL_FUEC_DbContext(DbContextOptions<GLOBAL_FUEC_DbContext> options)
        : base(options)
        {
        }

        public DbSet<Driver_Global> Driver_Globals { get; set; } // Nombre tabla sugerido: "Driver_Globals"
        public DbSet<Document_Driver_Global> Document_Driver_Globals { get; set; } // Nombre tabla sugerido: "Document_Driver_Globals"

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuraciones adicionales del modelo
            modelBuilder.Entity<Driver_Global>(entity =>
            {
                // Clave Primaria (PK) - int DriverGlobalRecordId (autoincremental)
                entity.HasKey(e => e.DriverGlobalRecordId);
                entity.Property(e => e.DriverGlobalRecordId)
                      .HasColumnType("INT"); // Indica que es autoincremental

                // TenantId - Clave para la lógica multi-inquilino
                entity.Property(e => e.TenantId)
                      .HasColumnType("VARCHAR(100)")
                      .HasMaxLength(100)
                      .IsRequired();

                // driver_Global_Id - Cédula (no es PK)
                entity.Property(e => e.driver_Global_Id)
                      .HasColumnType("VARCHAR(20)")
                      .HasMaxLength(20)
                      .IsRequired();

                // Índice compuesto para buscar por Tenant y Cédula eficientemente
                entity.HasIndex(e => new { e.TenantId, e.driver_Global_Id })
                      .IsUnique(false) // No es único globalmente, pero sí por inquilino (implícito)
                      .HasDatabaseName("IX_DriverGlobal_Tenant_DriverId"); // Nombre opcional del índice

                // Índice en driver_Global_Id solo (si necesitas buscar solo por cédula a través de todos los tenants)
                entity.HasIndex(e => e.driver_Global_Id)
                      .HasDatabaseName("IX_DriverGlobal_DriverId");

                // --- Propiedades comunes (basadas en Third_Party, Natural_Person) ---
                entity.Property(e => e.type_Id)
                      .HasColumnType("VARCHAR(30)") // Como Third_Party.type_Id
                      .HasMaxLength(30);

                entity.Property(e => e.image)
                      .HasColumnType("LONGTEXT")
                      .IsRequired(false);

                entity.Property(e => e.first_Name)
                      .HasColumnType("VARCHAR(100)") // Aumentado respecto a Natural_Person original
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.first_Last_Name)
                     .HasColumnType("VARCHAR(100)") // Aumentado
                     .HasMaxLength(100)
                     .IsRequired();

                entity.Property(e => e.second_Name)
                     .HasColumnType("VARCHAR(100)") // Aumentado
                     .HasMaxLength(100)
                     .IsRequired(false); // Permite nulos

                entity.Property(e => e.second_Last_Name)
                     .HasColumnType("VARCHAR(100)") // Aumentado
                     .HasMaxLength(100)
                     .IsRequired(false); // Permite nulos

                entity.Property(e => e.phone)
                      .HasColumnType("VARCHAR(20)") // Como Third_Party.phone
                      .HasMaxLength(20)
                      .IsRequired(false);

                entity.Property(e => e.email)
                      .HasColumnType("VARCHAR(100)") // Aumentado respecto a Third_Party original
                      .HasMaxLength(100)
                      .IsRequired(false);

                entity.Property(e => e.web_Site)
                      .HasColumnType("VARCHAR(200)") // Como Legal_Entity.web_Site
                      .HasMaxLength(200)
                      .IsRequired(false);

               
                

                // Relación Uno-a-Muchos: Un Driver_Global tiene muchos Document_Driver_Global
                entity.HasMany(d => d.Document_Driver_Globals)
                      .WithOne(doc => doc.Driver_Global) // Propiedad de navegación en Document_Driver_Global
                      .HasForeignKey(doc => doc.DriverGlobalRecordId); // FK en Document_Driver_Global
            });

            // --- Nueva Configuración para Document_Driver_Global ---
            modelBuilder.Entity<Document_Driver_Global>(entity =>
            {
               
                
                // Configura el campo 'id_Document' (el que envía el cliente)
                entity.Property(e => e.id_Document)
                      .HasColumnType("VARCHAR(100)") // Mantenemos el tipo y longitud
                      .HasMaxLength(100)
                      .IsRequired(); // Sigue siendo requerido

                // Clave Foránea (FK) a Driver_Global
                entity.Property(e => e.DriverGlobalRecordId)
                      .HasColumnType("INT") // Debe coincidir con Driver_Global.DriverGlobalRecordId
                      .IsRequired();

                // Otras propiedades
                entity.Property(e => e.type_Document)
                      .HasColumnType("VARCHAR(100)")
                      .HasMaxLength(100);

                entity.Property(e => e.start_validity)
                      .HasColumnType("DATETIME");

                entity.Property(e => e.end_validity)
                      .HasColumnType("DATETIME")
                      .IsRequired(false);

                entity.Property(e => e.image_Soport)
                      .HasColumnType("LONGTEXT")
                      .IsRequired(false);

                entity.HasIndex(e => new { e.DriverGlobalRecordId, e.id_Document })
                     .IsUnique(true)
                     .HasDatabaseName("IX_DocGlobal_DriverRecordId_DocId"); // Nombre opcional del índice



            });

            


        }
    }


}
