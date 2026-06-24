using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Aplicacion.Implementaciones;
using Aplicacion.Interfaces;
using Repositorio.Interfaces; 
using Repositorio.Implementaciones;
using ut_presentacion.Nucleo;
using System.Diagnostics;

namespace ut_presentacion.Aplicacion
{
    [TestClass]
    public class LibrosPrueba
    {
        private readonly ILibrosAplicacion? iAplicacion;
        private readonly IConexion? iConexion;
        private List<Libros>? lista;
        private Libros? entidad;
        private string? isbnGenerado;

        private int? editorialCreadaId;
        private int? paisCreadoId;
        private int? tipoCreadoId;

        public LibrosPrueba()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            iAplicacion = new LibrosAplicacion(iConexion);
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.AreEqual(true, Guardar());
            Assert.AreEqual(true, Modificar());
            Assert.AreEqual(true, Listar());
            Assert.AreEqual(true, Borrar());
        }

        public bool Guardar()
        {
            var unique = Guid.NewGuid().ToString("N").Substring(0, 8);
            var nombreEditorial = "TEST_EDITORIAL_" + unique;
            var nombrePais = "TEST_PAIS_" + unique;
            var nombreTipo = "TEST_TIPO_" + unique;

            var editorial = new Editoriales { Nombre_Editorial = nombreEditorial, Sitio_Web = "http://test" };
            this.iConexion!.Editoriales!.Add(editorial);
            this.iConexion.SaveChanges();
            editorialCreadaId = editorial.Id;

            var pais = new Paises { Nombre_Pais = nombrePais, Region = "TestRegion" };
            this.iConexion.Paises!.Add(pais);
            this.iConexion.SaveChanges();
            paisCreadoId = pais.Id;

            var tipo = new Tipos { Nombre_Tipo = nombreTipo };
            this.iConexion.Tipos!.Add(tipo);
            this.iConexion.SaveChanges();
            tipoCreadoId = tipo.Id;

            // ISBN único
            do
            {
                isbnGenerado = "TEST-" + Guid.NewGuid().ToString("N").Substring(0, 12);
            } while (this.iConexion.Libros!.Any(l => l.Isbn == isbnGenerado));

            var nuevo = new Libros
            {
                Editorial = editorialCreadaId!.Value,
                Pais = paisCreadoId!.Value,
                Tipo = tipoCreadoId!.Value,
                Isbn = isbnGenerado,
                Titulo = "Libro de prueba " + DateTime.UtcNow.Ticks,
                Edicion = "Primera",
                Fecha_Lanzamiento = DateTime.Now
            };

            this.iConexion.Libros!.Add(nuevo);
            this.iConexion.SaveChanges();

            // Recuperar la instancia rastreada por el contexto
            this.entidad = this.iConexion.Libros!.FirstOrDefault(l => l.Isbn == isbnGenerado);
            if (this.entidad == null) throw new Exception("No se pudo recuperar el libro insertado.");

            return true;
        }

        public bool Modificar()
        {
            // Recuperamos la instancia rastreada por el contexto
            var libro = this.iConexion!.Libros!.FirstOrDefault(l => l.Id == this.entidad!.Id);
            if (libro == null) throw new Exception("No se encontró el libro para modificar");

            // Mutamos la instancia ya rastreada y guardamos
            libro.Titulo = libro.Titulo + " - Modificado";
            libro.Edicion = "Segunda";

            this.iConexion.SaveChanges();

            this.entidad = libro;
            return true;
        }

        public bool Listar()
        {
            this.lista = this.iConexion!.Libros!.ToList();
            return lista.Count > 0;
        }

        public bool Borrar()
        {
            // Usamos Find para obtener la instancia rastreada (si existe)
            var libro = this.iConexion!.Libros!.Find(this.entidad!.Id);
            if (libro == null)
            {
                // Si Find no devolvió nada, intentamos FirstOrDefault (y si sigue null, error)
                libro = this.iConexion.Libros!.FirstOrDefault(l => l.Id == this.entidad!.Id);
                if (libro == null) throw new Exception("No se encontró el libro para eliminar");
            }

            // Comprobar existencias sin provocar warning
            var tieneExistencias = this.iConexion?.Existencias?.Any(e => e.Libro == libro.Id) ?? false;
            if (tieneExistencias) throw new Exception("No se puede eliminar el libro porque tiene existencias asociadas");

            // Remove sobre la instancia rastreada devuelta por Find/FirstOrDefault
            this.iConexion.Libros!.Remove(libro);
            this.iConexion.SaveChanges();

            // Intento de limpieza de dependencias creadas (con logging en Debug si falla)
            try
            {
                if (editorialCreadaId.HasValue)
                {
                    var used = this.iConexion?.Libros?.Any(l => l.Editorial == editorialCreadaId.Value) ?? false;
                    if (!used)
                    {
                        var ed = this.iConexion.Editoriales!.Find(editorialCreadaId.Value);
                        if (ed != null) { this.iConexion.Editoriales.Remove(ed); this.iConexion.SaveChanges(); }
                    }
                }

                if (paisCreadoId.HasValue)
                {
                    var used = this.iConexion?.Libros?.Any(l => l.Pais == paisCreadoId.Value) ?? false;
                    if (!used)
                    {
                        var p = this.iConexion.Paises!.Find(paisCreadoId.Value);
                        if (p != null) { this.iConexion.Paises.Remove(p); this.iConexion.SaveChanges(); }
                    }
                }

                if (tipoCreadoId.HasValue)
                {
                    var used = this.iConexion?.Libros?.Any(l => l.Tipo == tipoCreadoId.Value) ?? false;
                    if (!used)
                    {
                        var t = this.iConexion.Tipos!.Find(tipoCreadoId.Value);
                        if (t != null) { this.iConexion.Tipos.Remove(t); this.iConexion.SaveChanges(); }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Limpieza fallida en LibrosPrueba: {ex.Message}");
            }

            return true;
        }
    }
}
