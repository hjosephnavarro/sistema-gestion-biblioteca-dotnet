using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Dominio.Entidades
{
    public class Autores
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; } = null!;

        public string? Nacionalidad { get; set; }

        // Evita ciclos al serializar: Autor -> LibrosAutores -> AutorNavigation -> ...
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public ICollection<LibrosAutores>? LibrosAutores { get; set; } = new List<LibrosAutores>();
    }
}
