using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class EstadosExistencias
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Existencia { get; set; }

        [Required]
        public int Estado { get; set; }

        public DateTime Fecha_Cambio { get; set; } = DateTime.Now;

        [ForeignKey("Estado")]
        public Estados EstadoNavigation { get; set; } = null!;

        [ForeignKey("Existencia")]
        public Existencias ExistenciaNavigation { get; set; } = null!;
    }
}
