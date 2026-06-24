using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Sanciones
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Usuario { get; set; }

        [Required]
        public string Descripcion { get; set; } = null!;

        [Required]
        public DateTime Fecha_Inicio { get; set; }

        public DateTime? Fecha_Fin { get; set; }

        [ForeignKey("Usuario")]
        public Usuarios UsuarioNavigation { get; set; } = null!;
    }
}
