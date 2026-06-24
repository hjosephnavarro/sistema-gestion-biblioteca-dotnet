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
    public class ExistenciasPrueba
    {
        private readonly IExistenciasAplicacion? iAplicacion;
        private readonly IConexion? iConexion;
        private List<Existencias>? lista;
        private Existencias? entidad;

        public ExistenciasPrueba()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            iAplicacion = new ExistenciasAplicacion(iConexion);
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
            // Simulamos un libro existente con Id=1 (asegúrate de tenerlo en la BD)
            this.entidad = new Existencias
            {
                Libro = 1,
                Ejemplares = 5
            };

            this.iConexion!.Existencias!.Add(this.entidad);
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Modificar()
        {
            this.entidad!.Ejemplares = 10; // Cambiamos cantidad
            var entry = this.iConexion!.Entry<Existencias>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion.SaveChanges();
            return true;
        }

        public bool Listar()
        {
            this.lista = this.iConexion!.Existencias!
                .Include(e => e.LibroNavigation)
                .ToList();
            return lista.Count > 0;
        }

        public bool Borrar()
        {
            this.iConexion!.Existencias!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}