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
    public class EstadosPrueba
    {
        private readonly IEstadosAplicacion? iAplicacion;
        private readonly IConexion? iConexion;
        private List<Estados>? lista;
        private Estados? entidad;

        public EstadosPrueba()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            iAplicacion = new EstadosAplicacion(iConexion);
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.AreEqual(true, Guardar());
            Assert.AreEqual(true, Modificar());
            Assert.AreEqual(true, Listar());
            Assert.AreEqual(true, Borrar());
        }

        public bool Listar()
        {
            this.lista = this.iConexion!.Estados!.ToList();
            return lista.Count > 0;
        }

        public bool Guardar()
        {
            this.entidad = new Estados
            {
                Nombre_Estado = "EstadoPrueba"
            };

            this.iConexion!.Estados!.Add(this.entidad);
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Modificar()
        {
            this.entidad!.Nombre_Estado = "EstadoPruebaModificado";
            var entry = this.iConexion!.Entry<Estados>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            this.iConexion!.Estados!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}
