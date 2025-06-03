using ENTITY;
using ENTITY.Models;
using ENTITY.Models_Histories;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    
    public class FUEC_DbContext : DbContext
    {

        public FUEC_DbContext(DbContextOptions<FUEC_DbContext> options) : base(options) { }


        public FUEC_DbContext()
        {
        }
        public DbSet<Third_Party> Third_Party { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Natural_Person> Natural_Persons { get; set; }
        public DbSet<Legal_Entity> Legal_Entities { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Contracting_Party> Contracting_Partys { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Driver_History> Driver_Histories { get; set; }
        public DbSet<Document_Driver> Document_Drivers { get; set; }
        public DbSet<Document_Driver_History> Document_Driver_Histories { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuraciones adicionales del modelo.

            // Configuracion de Datos en la tabla third_party
            modelBuilder.Entity<Third_Party>(entity =>
            {
                entity.Property(e => e.thirdParty_Id)
                    .HasColumnType("VARCHAR(20)")
                    .HasMaxLength(20);

                entity.Property(e => e.type_Id)
                    .HasColumnType("VARCHAR(30)")
                    .HasMaxLength(30);

                entity.Property(e => e.type_Third_Party)
                    .HasColumnType("VARCHAR(20)")
                    .HasMaxLength(20);

                entity.Property(e => e.image)
                    .HasColumnType("LONGTEXT");

                entity.Property(e => e.phone)
                    .HasColumnType("VARCHAR(20)")
                    .HasMaxLength(20);

                entity.Property(e => e.email)
                    .HasColumnType("VARCHAR(100)")
                    .HasMaxLength(100);
               
                entity.Property(e => e.create_At)
                    .HasColumnType("DATETIME");
                entity.Property(e => e.update_At)
                    .HasColumnType("DATETIME");
                entity.Property(e => e.create_By)
                    .HasColumnType("VARCHAR(50)")
                    .HasMaxLength(50);
                entity.Property(e => e.update_By)
                    .HasColumnType("VARCHAR(50)")
                    .HasMaxLength(50);

            });

            // Configuracion de Datos en la tabla Address
            modelBuilder.Entity<Address>(entity =>
            {
                // Configuración del tipo de datos para cada 
                // Clave primaria
                entity.Property(e => e.id)
                    .HasColumnType("INT")
                    .ValueGeneratedOnAdd(); // Para 
                // Propiedades regulares
                entity.Property(e => e.street_Type)
                    .HasColumnType("VARCHAR(50)")
                    .HasMaxLength(50);

                entity.Property(e => e.street_Number)
                    .HasColumnType("INT");

                entity.Property(e => e.intersection_Number)
                    .HasColumnType("VARCHAR(20)")
                    .HasMaxLength(20);

                entity.Property(e => e.property_Number)
                    .HasColumnType("INT");

                entity.Property(e => e.neighborhood)
                    .HasColumnType("VARCHAR(100)")
                    .HasMaxLength(100);

                entity.Property(e => e.zip_Code)
                    .HasColumnType("VARCHAR(10)")
                    .HasMaxLength(10);

                entity.Property(e => e.municipality)
                    .HasColumnType("VARCHAR(100)")
                    .HasMaxLength(100);

                entity.Property(e => e.city)
                    .HasColumnType("VARCHAR(100)")
                    .HasMaxLength(100);

                // Clave foránea - DEBE COINCIDIR CON EL TIPO DE Third_Party.id (VARCHAR(20))
                entity.Property(e => e.third_Id)
                    .HasColumnType("VARCHAR(20)")
                    .HasMaxLength(20);

            });

            //Configuracion de las llaves foraneas de la tabla Address con la tabla Third_Party
            modelBuilder.Entity<Address>()
                .HasOne(p => p.Third_Party)
                .WithMany(b => b.Address)
                .HasForeignKey(p => p.third_Id);

            // Configuracion de Datos en la tabla Natural_Person
            modelBuilder.Entity<Natural_Person>(entity =>
            {
                entity.Property(e => e.first_Name)
                    .HasColumnType("VARCHAR(100)")
                    .HasMaxLength(100);
                entity.Property(e => e.second_Name)
                    .HasColumnType("VARCHAR(100)")
                    .HasMaxLength(100);
                entity.Property(e => e.first_Last_Name)
                    .HasColumnType("VARCHAR(100)")
                    .HasMaxLength(100);
                entity.Property(e => e.second_Last_Name)
                    .HasColumnType("VARCHAR(100)")
                    .HasMaxLength(100);
            });

            // Configuracion de Datos en la tabla Legal_Entity
            modelBuilder.Entity<Legal_Entity>(entity =>
            {
                // Columna para legal_Representative
                entity.Property(e => e.legal_Representative)
                    .HasColumnType("VARCHAR(200)")
                    .HasMaxLength(200);

                // Columna para name_Company
                entity.Property(e => e.name_Company)
                    .HasColumnType("VARCHAR(200)")
                    .HasMaxLength(200);

                // Columna para check_Digit
                entity.Property(e => e.check_Digit)
                    .HasColumnType("INT");

                // Columna para web_Site
                entity.Property(e => e.web_Site)
                    .HasColumnType("VARCHAR(200)")
                    .HasMaxLength(200);
            });

            // Configuración de Datos en la tabla Owner
            // Owner ⇄ Third_Party (1:1) sin nav prop en Third_Party
            modelBuilder.Entity<Owner>(entity =>
            {
                entity.Property(e => e.id_)
                    .HasColumnType("INT")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.type_Owner)
                    .HasColumnType("VARCHAR(40)")
                    .HasMaxLength(40);

                entity.Property(e => e.third_Id)
                    .HasColumnType("VARCHAR(20)")
                    .HasMaxLength(20);

                // Asegura unicidad en Owner.thirdParty_Id
                entity.HasIndex(e => e.third_Id)
                    .IsUnique();

                // Configura la FK 1:1 con Third_Party
                entity.HasOne(o => o.Third_Party)
                      .WithOne()  // sin lambda, no nav prop en Third_Party
                      .HasForeignKey<Owner>(o => o.third_Id)
                      .HasPrincipalKey<Third_Party>(tp => tp.thirdParty_Id);
            });


            // Configuración de Datos en la tabla Contracting_Party
            // Contracting_Party ⇄ Third_Party (1:1) sin nav prop en Third_Party
            modelBuilder.Entity<Contracting_Party>(entity =>
            {
                entity.Property(e => e.id_)
                    .HasColumnType("INT")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.signature_Image)
                    .HasColumnType("LONGTEXT");

                entity.Property(e => e.third_Id)
                    .HasColumnType("VARCHAR(20)")
                    .HasMaxLength(20);

                // Cada Contracting_Party enlaza a un Third_Party único
                entity.HasIndex(e => e.third_Id)
                      .IsUnique();
                // Configura la FK 1:1 con Third_Party
                entity.HasOne(o => o.Third_Party)
                      .WithOne()  // sin lambda, no nav prop en Third_Party
                      .HasForeignKey<Contracting_Party>(o => o.third_Id)
                      .HasPrincipalKey<Third_Party>(tp => tp.thirdParty_Id);
            });

            // Configuración de Datos en la tabla Driver
            modelBuilder.Entity<Driver>(entity =>
            {
                // Clave Primaria (PK) - string driver_Id (Cédula)
                entity.HasKey(e => e.driver_Id);
                entity.Property(e => e.driver_Id)
                      .HasColumnType("VARCHAR(20)") // Coincide con Third_Party.thirdParty_Id
                      .HasMaxLength(20);

                entity.Property(e => e.type_Id)
                  .HasColumnType("VARCHAR(30)") // Como  Third_Party.type_Id
                  .HasMaxLength(30)
                  .IsRequired();

                entity.Property(e => e.image)
                  .HasColumnType("LONGTEXT") // Para imágenes como URL o base64
                  .IsRequired(false); // Nullable

                entity.Property(e => e.first_Name)
                      .HasColumnType("VARCHAR(100)")
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.first_Last_Name)
                      .HasColumnType("VARCHAR(100)")
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.second_Name)
                      .HasColumnType("VARCHAR(100)")
                      .HasMaxLength(100)
                      .IsRequired(false); // Nullable

                entity.Property(e => e.second_Last_Name)
                      .HasColumnType("VARCHAR(100)")
                      .HasMaxLength(100)
                      .IsRequired(false); // Nullable

                entity.Property(e => e.phone)
                      .HasColumnType("VARCHAR(20)")
                      .HasMaxLength(20)
                      .IsRequired(false); // Nullable

                entity.Property(e => e.email)
                      .HasColumnType("VARCHAR(100)") // Ajusta longitud si es necesario
                      .HasMaxLength(100)
                      .IsRequired(false); // Nullable

                entity.Property(e => e.web_Site)
                      .HasColumnType("VARCHAR(200)")
                      .HasMaxLength(200)
                      .IsRequired(false); // Nullable

                entity.Property(e => e.create_At)
                       .HasColumnType("DATETIME")
                       .IsRequired();

                entity.Property(e => e.update_At)
                       .HasColumnType("DATETIME")
                       .IsRequired();

                entity.Property(e => e.create_By)
                      .HasColumnType("VARCHAR(100)") // Ajusta longitud según necesidad
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.update_By)
                      .HasColumnType("VARCHAR(100)") // Ajusta longitud según necesidad
                      .HasMaxLength(100)
                      .IsRequired();



                // Relación Uno-a-Muchos: Un Driver tiene muchos Document_Driver
                entity.HasMany(d => d.Document_Drivers)
                      .WithOne(doc => doc.Driver) // Propiedad de navegación en Document_Driver
                      .HasForeignKey(doc => doc.driver_Id); // FK en Document_Driver
            });

            // Configuración para Driver_History ---
            modelBuilder.Entity<Driver_History>(entity =>
            {
                // Clave Primaria
                entity.HasKey(e => e.HistoryId);
                entity.Property(e => e.HistoryId)
                      .HasColumnType("INT") // Para MySQL/MariaDB si quieres ser explícito
                      .ValueGeneratedOnAdd(); // Autoincremental

                // Cédula del Driver original al que pertenece este historial
                entity.Property(e => e.DriverAuditId)
                      .HasColumnType("VARCHAR(20)") // Igual que Driver.driver_Id
                      .HasMaxLength(20)
                      .IsRequired();

                // Campos de la acción de historial
                entity.Property(e => e.ActionTimestamp)
                      .HasColumnType("DATETIME")
                      .IsRequired();

                entity.Property(e => e.ActionType)
                      .HasColumnType("VARCHAR(50)")
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.ChangedByUserId)
                      .HasColumnType("VARCHAR(100)") // Asumiendo misma longitud que create_By/update_By
                      .HasMaxLength(100)
                      .IsRequired();

                // Copia de los campos de Driver (con tipos y restricciones consistentes)
                entity.Property(e => e.type_Id).HasColumnType("VARCHAR(30)").HasMaxLength(30).IsRequired();
                entity.Property(e => e.image).HasColumnType("LONGTEXT").IsRequired(false); // Nullable
                entity.Property(e => e.first_Name).HasColumnType("VARCHAR(100)").HasMaxLength(100).IsRequired();
                entity.Property(e => e.first_Last_Name).HasColumnType("VARCHAR(100)").HasMaxLength(100).IsRequired();
                entity.Property(e => e.second_Name).HasColumnType("VARCHAR(100)").HasMaxLength(100).IsRequired(false);
                entity.Property(e => e.second_Last_Name).HasColumnType("VARCHAR(100)").HasMaxLength(100).IsRequired(false);
                entity.Property(e => e.phone).HasColumnType("VARCHAR(20)").HasMaxLength(20).IsRequired(false);
                entity.Property(e => e.email).HasColumnType("VARCHAR(100)").HasMaxLength(100).IsRequired(false);
                entity.Property(e => e.web_Site).HasColumnType("VARCHAR(200)").HasMaxLength(200).IsRequired(false);
                entity.Property(e => e.state).IsRequired(); // bool

                entity.Property(e => e.create_At_OriginalDriver).HasColumnType("DATETIME").IsRequired();
                entity.Property(e => e.update_At_OriginalDriver).HasColumnType("DATETIME").IsRequired();
                entity.Property(e => e.create_By_OriginalDriver).HasColumnType("VARCHAR(100)").HasMaxLength(100).IsRequired();
                entity.Property(e => e.update_By_OriginalDriver).HasColumnType("VARCHAR(100)").HasMaxLength(100).IsRequired();

                entity.HasMany(dh => dh.SnapshottedDocuments) // Un Driver_History tiene muchos SnapshottedDocuments
                      .WithOne(ddh => ddh.DriverHistory)      // Cada Document_Driver_History tiene un DriverHistory (la nueva prop. de navegación)
                      .HasForeignKey(ddh => ddh.DriverHistoryId_FK) // Usando esta FK en Document_Driver_History
                      .OnDelete(DeleteBehavior.Cascade); // O la que prefieras (Cascade es común para historial)
                     


                // Índices para búsquedas comunes en la tabla de historial
                entity.HasIndex(e => e.DriverAuditId).HasDatabaseName("IX_DriverHistory_DriverAuditId");
                entity.HasIndex(e => e.ActionTimestamp).HasDatabaseName("IX_DriverHistory_ActionTimestamp");
                // Podrías considerar un índice compuesto si buscas frecuentemente por DriverAuditId Y ActionTimestamp
                // entity.HasIndex(e => new { e.DriverAuditId, e.ActionTimestamp }).HasDatabaseName("IX_DriverHistory_AuditId_Timestamp");



            });

            // Configuración para Document_Driver (Interno) ---
            modelBuilder.Entity<Document_Driver>(entity =>
            {
                // Clave Primaria (PK) - string id_Document
                entity.HasKey(e => e.id_Document);
                entity.Property(e => e.id_Document)
                      .HasColumnType("VARCHAR(100)")
                      .HasMaxLength(100);

                // Clave Foránea (FK) a Driver
                entity.Property(e => e.driver_Id)
                      .HasColumnType("VARCHAR(20)") // Debe coincidir con Driver.driver_Id
                      .HasMaxLength(20)
                      .IsRequired();

                // Otras propiedades
                entity.Property(e => e.type_Document)
                      .HasColumnType("VARCHAR(100)")
                      .HasMaxLength(100);

                entity.Property(e => e.start_validity)
                      .HasColumnType("DATETIME"); // O DATE si no necesitas hora

                entity.Property(e => e.end_validity)
                      .HasColumnType("DATETIME") // O DATE
                      .IsRequired(false); // Permite nulos

                entity.Property(e => e.image_Soport)
                      .HasColumnType("LONGTEXT")
                      .IsRequired(false);
            });

            // Configuración para Document_Driver_History ---
            modelBuilder.Entity<Document_Driver_History>(entity =>
            {
                // Clave Primaria
                entity.HasKey(e => e.DocHistoryId);
                entity.Property(e => e.DocHistoryId)
                      .HasColumnType("INT")
                      .ValueGeneratedOnAdd(); // Autoincremental

                // ID del Document_Driver original al que pertenece este historial
                entity.Property(e => e.DocumentAuditId)
                      .HasColumnType("VARCHAR(100)") // Igual que Document_Driver.id_Document
                      .HasMaxLength(100)
                      .IsRequired();

                // Campos de la acción de historial
                entity.Property(e => e.ActionTimestamp)
                      .HasColumnType("DATETIME")
                      .IsRequired();

                entity.Property(e => e.ActionType)
                      .HasColumnType("VARCHAR(50)")
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.ChangedByUserId)
                      .HasColumnType("VARCHAR(100)") // Consistente con otros campos 'By'
                      .HasMaxLength(100)
                      .IsRequired();

                // FK a la tabla Driver_History
                entity.Property(e => e.DriverHistoryId_FK)
                      .HasColumnType("INT")
                      .IsRequired();

               
                // Campos copiados de Document_Driver
                entity.Property(e => e.type_Document).HasColumnType("VARCHAR(100)").HasMaxLength(100);
                entity.Property(e => e.start_validity).HasColumnType("DATETIME");
                entity.Property(e => e.end_validity).HasColumnType("DATETIME").IsRequired(false);
                entity.Property(e => e.image_Soport).HasColumnType("LONGTEXT").IsRequired(false);
                // entity.Property(e => e.is_Expirable); // bool, usualmente no necesita config de tipo

                entity.Property(e => e.driver_Id_Original) // Cédula del driver original
                      .HasColumnType("VARCHAR(20)")
                      .HasMaxLength(20)
                      .IsRequired();

                // Índices
                entity.HasIndex(e => e.DocumentAuditId).HasDatabaseName("IX_DocHist_DocAuditId");
                entity.HasIndex(e => e.DriverHistoryId_FK).HasDatabaseName("IX_DocHist_DriverHistoryIdFK");
                // Podrías considerar un índice compuesto si buscas frecuentemente por ambos
                // entity.HasIndex(e => new { e.DriverHistoryId_FK, e.DocumentAuditId }).HasDatabaseName("IX_DocHist_DriverHistId_DocAuditId");
            });



            // Configuracion Herencia de la tabla Third_Party con natural person y legal entity
            modelBuilder.Entity<Third_Party>()
                .HasDiscriminator<string>("type_Third_Party")
                .HasValue<Natural_Person>("Persona Natural")
                .HasValue<Legal_Entity>("Persona Juridica");
            
        }
            



    }
}
