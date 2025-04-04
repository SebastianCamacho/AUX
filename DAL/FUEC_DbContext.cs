using ENTITY;
using ENTITY.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    
    public class FUEC_DbContext : DbContext
    {

        public FUEC_DbContext(DbContextOptions<FUEC_DbContext> options) : base(options) { }


        public FUEC_DbContext()
        {
        }

        public DbSet<Address> Address { get; set; }
        public DbSet<Natural_Person> Natural_Persons { get; set; }
        public DbSet<Legal_Entity> Legal_Entitys { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Contracting_Party> Contracting_Partys { get; set; }


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
                
                entity.Property(e => e.phone)
                    .HasColumnType("VARCHAR(20)")
                    .HasMaxLength(20);
                entity.Property(e => e.email)
                    .HasColumnType("VARCHAR(50)")
                    .HasMaxLength(50);
               
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
                    .HasColumnType("VARCHAR(30)")
                    .HasMaxLength(30);
                entity.Property(e => e.second_Name)
                    .HasColumnType("VARCHAR(30)")
                    .HasMaxLength(30);
                entity.Property(e => e.first_Last_Name)
                    .HasColumnType("VARCHAR(30)")
                    .HasMaxLength(30);
                entity.Property(e => e.second_Last_Name)
                    .HasColumnType("VARCHAR(30)")
                    .HasMaxLength(30);
            });

            // Configuracion de Datos en la tabla Legal_Entity
            modelBuilder.Entity<Legal_Entity>(entity =>
            {
                entity.Property(e => e.legal_Representative)
                    .HasColumnType("VARCHAR(200)")
                    .HasMaxLength(200);
                
                
              
            });

            

            

            // Configuracion Herencia de la tabla Third_Party con natural person y legal entity
            modelBuilder.Entity<Third_Party>()
                .HasDiscriminator<string>("type_Third_Party")
                .HasValue<Natural_Person>("Persona Natural")
                .HasValue<Legal_Entity>("Persona Juridica");
            
        }
            



    }
}
