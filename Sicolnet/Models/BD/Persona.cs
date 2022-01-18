using Sicolnet.Models.BD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sicolnet.Models.BD
{




    public class Persona
    {
        [Key]
        public int IdPersona { get; set; }

        public string Cedula { get; set; }

        public string Nombres { get; set; }

        public string Apellidos { get; set; }


        public string Celular { get; set; }

        public string Email { get; set; }

        public int IdMunicipio { get; set; }

        //public DateTime FechaNacimiento { get; set; }

        public int IdReferente { get; set; }

        public int IdEstado { get; set; }

        [DefaultValue("")]
        public string ShortUrl { get; set; }

        [DefaultValue(" ")]
        public string ShortUrlToken { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime FechaUltimaModificacion { get; set; }
        
        [ForeignKey("IdReferente")]
        public Persona Referente { get; set; }

        [ForeignKey("IdMunicipio")]
        public Municipio Municipio { get; set; } 
    }
}
