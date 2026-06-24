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
    public class EstadosExistenciasPrueba
    {
        private readonly IEstadosExistenciasAplicacion? iAplicacion;
        private readonly IConexion? iConexion;
        private List<EstadosExistencias>? lista;
        private EstadosExistencias? entidad;

        public EstadosExistenciasPrueba()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            iAplicacion = new EstadosExistenciasAplicacion(iConexion);
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
            // Simulamos: Existencia=1, Estado=1 (asegúrate que existan en la BD)
            this.entidad = new EstadosExistencias
            {
                Existencia = 1,
                Estado = 1
            };

            this.iConexion!.EstadosExistencias!.Add(this.entidad);
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Modificar()
        {
            this.entidad!.Estado = 2; // Cambiamos el estado
            var entry = this.iConexion!.Entry<EstadosExistencias>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion.SaveChanges();
            return true;
        }

        public bool Listar()
        {
            this.lista = this.iConexion!.EstadosExistencias!
                .Include(e => e.EstadoNavigation)
                .Include(e => e.ExistenciaNavigation)
                .ToList();
            return lista.Count > 0;
        }

        public bool Borrar()
        {
            this.iConexion!.EstadosExistencias!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}
