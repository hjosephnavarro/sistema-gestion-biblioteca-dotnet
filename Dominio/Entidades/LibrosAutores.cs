using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class LibrosAutores
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Libro { get; set; }

        [Required]
        public int Autor { get; set; }

        [ForeignKey("Autor")]
        public Autores AutorNavigation { get; set; } = null!;

        [ForeignKey("Libro")]
        // Evita ciclos: no volver a serializar el libro desde la relación
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Libros LibroNavigation { get; set; } = null!;
    }
}
