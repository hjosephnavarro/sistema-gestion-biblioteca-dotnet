using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Dominio.Entidades
{
    public class Libros
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Editorial { get; set; }

        [Required]
        public int Pais { get; set; }

        [Required]
        public int Tipo { get; set; }

        [Required, StringLength(20)]
        public string Isbn { get; set; } = null!;

        [Required, StringLength(200)]
        public string Titulo { get; set; } = null!;

        [StringLength(50)]
        public string? Edicion { get; set; }

        public DateTime? Fecha_Lanzamiento { get; set; }

        [ForeignKey("Editorial")]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Editoriales EditorialNavigation { get; set; } = null!;

        [ForeignKey("Pais")]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Paises PaisNavigation { get; set; } = null!;

        [ForeignKey("Tipo")]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Tipos TipoNavigation { get; set; } = null!;

        public ICollection<Existencias> Existencias { get; set; } = new List<Existencias>();
        public ICollection<LibrosAutores> LibrosAutores { get; set; } = new List<LibrosAutores>();
        public ICollection<LibrosTemas> LibrosTemas { get; set; } = new List<LibrosTemas>();
    }
}
