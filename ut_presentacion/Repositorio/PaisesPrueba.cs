using Dominio.Entidades;
using Repositorio.Implementaciones;
using Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class PaisesPrueba
    {
        private readonly IConexion? iConexion;
        private List<Paises>? lista;
        private Paises? entidad;

        public PaisesPrueba()
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
            this.entidad = EntidadesNucleo.Paises()!;
            iConexion!.Paises!.Add(this.entidad);
            iConexion.SaveChanges();
            return true;
        }

        public bool Modificar()
        {
            this.entidad!.Region = "América Latina";
            var entry = iConexion!.Entry<Paises>(this.entidad);
            entry.State = EntityState.Modified;
            iConexion.SaveChanges();
            return true;
        }

        public bool Listar()
        {
            this.lista = iConexion!.Paises!.ToList();
            return lista.Count > 0;
        }

        public bool Borrar()
        {
            iConexion!.Paises!.Remove(this.entidad!);
            iConexion.SaveChanges();
            return true;
        }
    }
}

