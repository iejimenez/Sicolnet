using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sicolnet.Models.BD
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int IdPersona { get; set; }

        [ForeignKey("IdPersona")]
        public virtual Persona Tercero { get; set; }
        
    }
}
