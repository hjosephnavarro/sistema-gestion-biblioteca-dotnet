using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
public class Prestamos
{
        [Key]
        public int Id { get; set; }

        [Required]
        public int Usuario { get; set; }

        [Required]
        public int Tipo_Prestamo { get; set; }

        [Required]
        public int Existencia { get; set; }

        [Required]
        public DateTime Fecha_Prestamo { get; set; }

        public DateTime? Fecha_Devolucion { get; set; }
        public DateTime? Fecha_Entrega_Real { get; set; }

        [ForeignKey("Existencia")]
        public Existencias ExistenciaNavigation { get; set; } = null!;

        [ForeignKey("Tipo_Prestamo")]
        public TiposPrestamos TipoPrestamoNavigation { get; set; } = null!;

        [ForeignKey("Usuario")]
        public Usuarios UsuarioNavigation { get; set; } = null!;
    }
}