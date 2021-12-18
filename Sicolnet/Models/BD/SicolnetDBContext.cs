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

        public List<Persona> GetPersonaByCedulaOrNombre(string query)
        {
            return this.Personas.Where(p => p.Cedula.Trim().Contains(query) || p.Nombres.Trim().Contains(query) || p.Apellidos.Trim().Contains(query)).ToList();
        }

        public Persona InsertarPersona(Persona persona)
        {
            this.Personas.Add(persona);
            this.SaveChanges();
            return persona;
        }

        public Persona UpdatePersona(Persona persona)
        {
            this.Personas.Update(persona);
            this.SaveChanges();
            return persona;
        }
        #endregion

        #region Admins
        public List<Usuario> GetAdmins()
        {
            return this.Usuarios.ToList();
        }
        public Usuario InsertarUsuario(Usuario user)
        {
            this.Usuarios.Add(user);
            this.SaveChanges();
            return user;
        }
        #endregion

    }
}
