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
    public class SancionesPrueba
    {
        private readonly ISancionesAplicacion? iAplicacion;
        private readonly IConexion? iConexion;
        private List<Sanciones>? lista;
        private Sanciones? entidad;

        // FK Usuario
        private Usuarios? usuarioTest;

        public SancionesPrueba()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            iAplicacion = new SancionesAplicacion(iConexion);
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.AreEqual(true, PrepararDependencias());
            Assert.AreEqual(true, Guardar());
            Assert.AreEqual(true, Modificar());
            Assert.AreEqual(true, Listar());
            Assert.AreEqual(true, Borrar());
        }

        /// <summary>
        /// Prepara el usuario necesario para la FK de la sanción
        /// </summary>
        public bool PrepararDependencias()
        {
            // Usuario de prueba
            this.usuarioTest = EntidadesNucleo.Usuarios() ?? new Usuarios();
            this.iConexion!.Usuarios!.Add(this.usuarioTest);
            this.iConexion.SaveChanges();

            return usuarioTest.Id != 0;
        }

        public bool Guardar()
        {
            // Crear sanción de prueba
            this.entidad = new Sanciones
            {
                Usuario = this.usuarioTest!.Id,
                Descripcion = "Sanción de prueba",
                Fecha_Inicio = DateTime.UtcNow,
                Fecha_Fin = DateTime.UtcNow.AddDays(7)
            };

            var resultado = this.iAplicacion!.Guardar(this.entidad);
            this.entidad = resultado;
            return resultado != null && this.entidad!.Id != 0;
        }

        public bool Modificar()
        {
            if (this.entidad == null) return false;

            // Modificar una propiedad simple para prueba
            this.entidad.Descripcion = "Sanción modificada";
            var resultado = this.iAplicacion!.Modificar(this.entidad);
            return resultado != null && resultado.Descripcion == this.entidad.Descripcion;
        }

        public bool Listar()
        {
            this.lista = this.iAplicacion!.Listar();
            return lista != null && lista.Count > 0;
        }

        public bool Borrar()
        {
            if (this.entidad == null) return false;
            var resultado = this.iAplicacion!.Borrar(this.entidad);
            return resultado != null;
        }
    }
}
