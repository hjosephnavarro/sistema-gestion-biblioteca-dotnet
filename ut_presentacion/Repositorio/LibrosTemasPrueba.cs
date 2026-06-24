using Dominio.Entidades;
using Repositorio.Implementaciones;
using Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class LibrosTemasPrueba
    {
        private readonly IConexion? iConexion;
        private List<LibrosTemas>? lista;
        private LibrosTemas? entidad;

        private Libros? libro;
        private Temas? tema;

        public LibrosTemasPrueba()
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
            // Crear dependencias de Libro
            var editorial = EntidadesNucleo.Editoriales()!;
            var pais = EntidadesNucleo.Paises()!;
            var tipo = EntidadesNucleo.Tipos()!;

            this.iConexion!.Editoriales!.Add(editorial);
            this.iConexion!.Paises!.Add(pais);
            this.iConexion!.Tipos!.Add(tipo);
            this.iConexion!.SaveChanges();

            // Crear Tema
            this.tema = EntidadesNucleo.Temas()!;
            this.iConexion!.Temas!.Add(this.tema);
            this.iConexion!.SaveChanges();

            // Crear libro con ISBN único
            string isbnUnico = "ISBN-" + Guid.NewGuid().ToString("N").Substring(0, 13);
            this.libro = new Libros
            {
                Editorial = editorial.Id,
                Pais = pais.Id,
                Tipo = tipo.Id,
                Isbn = isbnUnico,
                Titulo = "Libro de prueba con tema",
                Edicion = "Primera",
                Fecha_Lanzamiento = DateTime.Now
            };
            this.iConexion!.Libros!.Add(this.libro);
            this.iConexion!.SaveChanges();

            this.entidad = new LibrosTemas
            {
                Libro = this.libro.Id,
                Tema = this.tema.Id
            };
            this.iConexion!.LibrosTemas!.Add(this.entidad);
            this.iConexion!.SaveChanges();

            return true;
        }

        public bool Modificar()
        {
            var nuevoTema = EntidadesNucleo.Temas()!;
            this.iConexion!.Temas!.Add(nuevoTema);
            this.iConexion!.SaveChanges();

            this.entidad!.Tema = nuevoTema.Id;

            var entry = this.iConexion!.Entry<LibrosTemas>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();

            return true;
        }

        public bool Listar()
        {
            this.lista = this.iConexion!.LibrosTemas!.ToList();
            return lista.Count > 0;
        }

        public bool Borrar()
        {
            this.iConexion!.LibrosTemas!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}
