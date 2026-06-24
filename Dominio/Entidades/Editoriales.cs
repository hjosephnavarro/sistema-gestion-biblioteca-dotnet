using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text.Json.Serialization; // Manténlo
using Newtonsoft.Json; // Manténlo

namespace Dominio.Entidades
{
    public class Editoriales
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre_Editorial { get; set; } = null!;

        [StringLength(200)]
        public string? Sitio_Web { get; set; }

        //Evita la ambigüedad usando los nombres completos
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public ICollection<Libros> Libros { get; set; } = new List<Libros>();
    }
}
