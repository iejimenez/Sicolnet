using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sicolnet.Models.BD
{
    public class SicolnetDBContext : DbContext
    {
        public SicolnetDBContext()
        {
        }

        public SicolnetDBContext(DbContextOptions<SicolnetDBContext> options)
        : base(options)
        {
            
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }

        public DbSet<Municipio> Municipios { get; set; }

        public DbSet<Persona> Personas { get; set; }

        public DbSet<Token> Tokens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Token>()
                .HasKey(c => new { c.Cedula, c.Celular });
        }


        public List<Municipio> GetMunicipios()
        {
            return this.Municipios.ToList();
        }

        public List<Departamento> GetDepartamentos()
        {
            return this.Departamentos.ToList();
        }


        #region Persona

        public Persona InsertarPersona(Persona persona)
        {
            this.Personas.Add(persona);
            this.SaveChanges();
            return persona;
        }
        #endregion

    }
}
