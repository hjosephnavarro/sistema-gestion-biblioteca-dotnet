using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entidades
{
    public class Temas
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre_Tema { get; set; } = null!;

        [StringLength(100)]
        public string? Area_Conocimiento { get; set; }

        // Evita ciclos al serializar: Tema -> LibrosTemas -> TemaNavigation -> ...
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public ICollection<LibrosTemas> LibrosTemas { get; set; } = new List<LibrosTemas>();
    }
}
