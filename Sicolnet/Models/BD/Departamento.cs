using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sicolnet.Models.BD
{
    public class Departamento
    {
        [Key]

        public int IdDepartamento { get; set; }

        public string Nombre { get; set; }

        public string CodigoDane { get; set; }

        public virtual ICollection<Municipio> Municipios { get; set; }

    }
}
