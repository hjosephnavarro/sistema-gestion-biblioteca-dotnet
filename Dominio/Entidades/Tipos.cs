using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entidades
{
    public class Tipos
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre_Tipo { get; set; } = null!;

        // Ignorar la colección inversa para evitar ciclos al serializar Libros -> TipoNavigation -> Tipo.Libros -> ...
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public ICollection<Libros> Libros { get; set; } = new List<Libros>();
    }
}
