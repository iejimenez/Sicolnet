using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sicolnet.Models.Dtos
{
    public class MunicipioDto
    {
        public int IdMunicipio { get; set; }

        public string Nombre { get; set; }

        public string CodigoDane { get; set; }

        public int IdDepartamento { get; set; }

        public DepartamentoDto Departamento { get; set; }
    }
}
