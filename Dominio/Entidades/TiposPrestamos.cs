using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class TiposPrestamos
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Descripcion { get; set; } = null!;

        public ICollection<Prestamos> Prestamos { get; set; } = new List<Prestamos>();
    }
}
