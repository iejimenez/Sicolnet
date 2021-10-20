using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sicolnet.Models.Dtos
{
    public class UsuarioDto
    {
        public int IdUsuario { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int IdPersona { get; set; }

        public virtual PersonaDto Tercero { get; set; }
    }
}
