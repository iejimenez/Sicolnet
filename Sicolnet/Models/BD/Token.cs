using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sicolnet.Models.BD
{
    public class Token
    {
        public string Cedula { get; set; }

        public string Celular { get; set; }

        public int Key { get; set; }

        public DateTime FechaRegistro { get; set; }
    }
}
