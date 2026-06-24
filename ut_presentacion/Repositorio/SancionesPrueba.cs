using Dominio.Entidades;
using Repositorio.Implementaciones;
using Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class SancionesPrueba
    {
        private readonly IConexion? iConexion;
        private List<Sanciones>? lista;
        private Sanciones? entidad;
        private Usuarios? usuario;

        public SancionesPrueba()
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
            // Crear usuario dependiente
            this.usuario = EntidadesNucleo.Usuarios()!;
            iConexion!.Usuarios!.Add(this.usuario);
            iConexion.SaveChanges();

            // Crear sanción
            this.entidad = new Sanciones
            {
                Usuario = this.usuario.Id,
                Descripcion = "Retraso en devolución",
                Fecha_Inicio = DateTime.Now,
                Fecha_Fin = DateTime.Now.AddDays(3)
            };

            iConexion.Sanciones!.Add(this.entidad);
            iConexion.SaveChanges();

            return true;
        }

        public bool Modificar()
        {
            this.entidad!.Descripcion = "Multa actualizada";
            var entry = iConexion!.Entry<Sanciones>(this.entidad);
            entry.State = EntityState.Modified;
            iConexion.SaveChanges();
            return true;
        }

        public bool Listar()
        {
            this.lista = iConexion!.Sanciones!.ToList();
            return lista != null && lista.Count > 0;
        }

        public bool Borrar()
        {
            iConexion!.Sanciones!.Remove(this.entidad!);
            iConexion.SaveChanges();
            return true;
        }
    }
}
