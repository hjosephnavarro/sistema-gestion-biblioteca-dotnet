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
    public class AutoresPrueba
    {
        private readonly IAutoresAplicacion? iAplicacion;
        private readonly IConexion? iConexion;
        private List<Autores>? lista;
        private Autores? entidad;

        public AutoresPrueba()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            iAplicacion = new AutoresAplicacion(iConexion);
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
            this.entidad = EntidadesNucleo.Autores()!;
            this.entidad.Nombre = "AutorPrueba_" + Guid.NewGuid().ToString("N").Substring(0, 6);
            this.entidad.Nacionalidad = "Colombiana";

            this.iAplicacion!.Guardar(this.entidad);
            return true;
        }

        public bool Modificar()
        {
            // Desvincula la entidad previa del contexto
            this.iConexion!.Entry(this.entidad!).State = EntityState.Detached;

            var autor = this.entidad!;
            autor.Nombre = "AutorModificado_" + Guid.NewGuid().ToString("N").Substring(0, 6);
            autor.Nacionalidad = "Mexicana";

            this.iAplicacion!.Modificar(autor);
            this.entidad = autor;

            return true;
        }

        public bool Listar()
        {
            this.lista = this.iAplicacion!.Listar();
            return this.lista != null && this.lista.Count > 0;
        }

        public bool Borrar()
        {
            // Cierra el contexto anterior (solo si la clase concreta lo permite)
            (this.iConexion as IDisposable)?.Dispose();

            // Crea una nueva conexión limpia (nuevo DbContext)
            var nuevaConexion = new Conexion();
            nuevaConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");

            // Reasigna la conexión limpia a la aplicación
            var nuevaAplicacion = new AutoresAplicacion(nuevaConexion);

            // Crea una entidad temporal solo con el Id a eliminar
            var autor = new Autores { Id = this.entidad!.Id };

            nuevaAplicacion.Borrar(autor);
            return true;
        }
    }
}
