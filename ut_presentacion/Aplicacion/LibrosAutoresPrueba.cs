using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Aplicacion.Implementaciones;
using Aplicacion.Interfaces;
using Repositorio.Interfaces; 
using Repositorio.Implementaciones;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Aplicacion
{
    [TestClass]
    public class LibrosAutoresPrueba
    {
        private readonly ILibrosAutoresAplicacion? iAplicacion;
        private readonly IConexion? iConexion;
        private List<LibrosAutores>? lista;
        private LibrosAutores? entidad;

        public LibrosAutoresPrueba()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            iAplicacion = new LibrosAutoresAplicacion(iConexion);
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
            // Asegúrate de tener en BD un Libro con Id=1 y un Autor con Id=1
            this.entidad = new LibrosAutores
            {
                Libro = 1,
                Autor = 1
            };

            this.iConexion!.LibrosAutores!.Add(this.entidad);
            this.iConexion.SaveChanges();
            return true;
        }

        public bool Modificar()
        {
            // Supongamos que cambiamos el autor (Id=2)
            this.entidad!.Autor = 2;
            var entry = this.iConexion!.Entry<LibrosAutores>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion.SaveChanges();
            return true;
        }

        public bool Listar()
        {
            this.lista = this.iConexion!.LibrosAutores!
                .Include(la => la.AutorNavigation)
                .Include(la => la.LibroNavigation)
                .ToList();
            return lista.Count > 0;
        }

        public bool Borrar()
        {
            this.iConexion!.LibrosAutores!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}
