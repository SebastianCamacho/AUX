using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class Plataforma_Context:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Profesor> Profesores { get; set; }
        public DbSet<Administrador> Administradors { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<Asignatura> Asignaturas { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<Clase> Clases { get; set; }
        public DbSet<Asistencia> Asistencias { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Asistencia>().HasKey(x => new { x.EstudianteIdentificacion,x.idClase});

            
            modelBuilder.Entity<Matricula>().HasKey(x => new { x.EstudianteIdentificacion, x.GrupoidGrupo });

            //Configuracion de Herencia
            modelBuilder.Entity<Usuario>()
            .HasDiscriminator<string>("Rol")
            .HasValue<Estudiante>("Estudiante")
            .HasValue<Profesor>("Profesor");

            // Configuración de la relación 1 a 1 entre User y Usuario
            modelBuilder.Entity<User>()
                .HasOne(u => u.Usuario) // Un User tiene un Usuario relacionado
                .WithOne(u => u.user)               // Un Usuario tiene un User relacionado
                .HasForeignKey<User>(u => u.UsuarioIdentificacion) // La clave foránea en User
                .HasPrincipalKey<Usuario>(u => u.Identificacion); // La clave principal en Usuario

            modelBuilder.Entity<Asistencia>()
                .HasOne(g => g.Clase) // Configuramos la relación 1 a muchos
                .WithMany(p => p.Asistencias)
                .HasForeignKey(g => g.idClase);

            // Relación entre Profesor y Grupos (eliminación en cascada)
            modelBuilder.Entity<Grupo>()
                .HasOne(a => a.profesor)
                .WithMany(u => u.Grupos)
                .HasForeignKey(a => a.ProfesorIdentificacion)
                .OnDelete(DeleteBehavior.Restrict);  // Si el Usuario es eliminado, la Asistencia también lo es
            
            // Relación entre Profesor y Grupos (eliminación en cascada)
            modelBuilder.Entity<Grupo>()
                .HasOne(a => a.profesor)
                .WithMany(u => u.Grupos)
                .HasForeignKey(a => a.ProfesorIdentificacion)
                .OnDelete(DeleteBehavior.Restrict);  // Si el Usuario es eliminado, la Asistencia también lo es



            base.OnModelCreating(modelBuilder);
        }

        public Plataforma_Context(DbContextOptions<Plataforma_Context> options) : base(options)
        {

        }

    }
}
