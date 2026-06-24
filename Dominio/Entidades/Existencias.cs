using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Existencias
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Libro { get; set; }

        [Required]
        public int Ejemplares { get; set; }

        [ForeignKey("Libro")]
        public Libros LibroNavigation { get; set; } = null!;

        public ICollection<EstadosExistencias> EstadosExistencia { get; set; } = new List<EstadosExistencias>();
        public ICollection<Prestamos> Prestamos { get; set; } = new List<Prestamos>();
    }
}
