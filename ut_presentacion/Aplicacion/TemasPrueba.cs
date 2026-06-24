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
    public class TemasPrueba
    {
        private readonly ITemasAplicacion? iAplicacion;
        private readonly IConexion? iConexion;
        private List<Temas>? lista;
        private Temas? entidad;

        public TemasPrueba()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            iAplicacion = new TemasAplicacion(iConexion);
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
            this.entidad = EntidadesNucleo.Temas() ?? new Temas
            {
                Nombre_Tema = "Tema de prueba",
                Area_Conocimiento = "Área de prueba"
            };

            var resultado = this.iAplicacion!.Guardar(this.entidad);
            this.entidad = resultado;
            return resultado != null && this.entidad!.Id != 0;
        }

        public bool Modificar()
        {
            if (this.entidad == null) return false;

            this.entidad.Nombre_Tema = "Tema modificado";
            var resultado = this.iAplicacion!.Modificar(this.entidad);
            return resultado != null && resultado.Nombre_Tema == this.entidad.Nombre_Tema;
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
