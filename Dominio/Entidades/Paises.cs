using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization; // opcional si usas System.Text.Json
// no es obligatorio quitar Newtonsoft en este archivo

namespace Dominio.Entidades
{
    public class Paises
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre_Pais { get; set; } = null!;

        [StringLength(100)]
        public string? Region { get; set; }

        // Evita ciclos: ignorar para ambos serializadores
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public ICollection<Libros> Libros { get; set; } = new List<Libros>();
    }
}
