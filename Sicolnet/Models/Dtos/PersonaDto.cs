using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sicolnet.Models.Dtos
{
    public class PersonaDto
    {
        public int IdPersona { get; set; }

        public string Cedula { get; set; }

        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        public string Celular { get; set; }

        public string Email { get; set; }

        public int IdMunicipio { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public int IdReferente { get; set; }

        public int IdEstado { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime FechaUltimaModificacion { get; set; }

        public PersonaDto Referente { get; set; }

        public MunicipioDto Municipio { get; set; }
    }
}
