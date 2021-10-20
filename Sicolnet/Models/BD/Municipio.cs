using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sicolnet.Models.BD
{
    public class Municipio
    {
        [Key]
        public int IdMunicipio { get; set; }

        public string Nombre { get; set; }

        public string CodigoDane { get; set; }

        public int IdDepartamento { get; set; }

        [ForeignKey("IdDepartamento")]
        public Departamento Departamento { get; set; }


    }
}
