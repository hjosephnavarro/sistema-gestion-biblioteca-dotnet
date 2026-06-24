using Dominio.Entidades;
using Repositorio.Implementaciones;
using Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class LibrosPrueba
    {
        private readonly IConexion? iConexion;
        private List<Libros>? lista;
        private Libros? entidad;

        public LibrosPrueba()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.IsTrue(Guardar());
            Assert.IsTrue(Modificar());
            Assert.IsTrue(Listar());
            Assert.IsTrue(Borrar());
        }

        public bool Guardar()
        {
            // Crear dependencias
            var editorial = EntidadesNucleo.Editoriales()!;
            var pais = EntidadesNucleo.Paises()!;
            var tipo = EntidadesNucleo.Tipos()!;

            this.iConexion!.Editoriales!.Add(editorial);
            this.iConexion!.Paises!.Add(pais);
            this.iConexion!.Tipos!.Add(tipo);
            this.iConexion!.SaveChanges();

            // Crear un ISBN único para evitar errores de constraint
            string isbnUnico = "ISBN-" + Guid.NewGuid().ToString("N").Substring(0, 13);

            // Crear libro
            this.entidad = new Libros
            {
                Editorial = editorial.Id,
                Pais = pais.Id,
                Tipo = tipo.Id,
                Isbn = isbnUnico,
                Titulo = "Libro de prueba",
                Edicion = "Primera",
                Fecha_Lanzamiento = DateTime.Now
            };

            this.iConexion!.Libros!.Add(this.entidad);
            this.iConexion!.SaveChanges();

            return true;
        }

        public bool Modificar()
        {
            this.entidad!.Titulo = "Libro Modificado";
            this.entidad!.Edicion = "Segunda";

            var entry = this.iConexion!.Entry<Libros>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();

            return true;
        }

        public bool Listar()
        {
            this.lista = this.iConexion!.Libros!.ToList();
            return lista.Count > 0;
        }

        public bool Borrar()
        {
            this.iConexion!.Libros!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}
