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
    public class PaisesPrueba
    {
        private readonly IPaisesAplicacion? iAplicacion;
        private readonly IConexion? iConexion;
        private List<Paises>? lista;
        private Paises? entidad;

        public PaisesPrueba()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            iAplicacion = new PaisesAplicacion(iConexion);
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
            // Crear una entidad de prueba (aseg�rate que cumple las anotaciones [Required], [StringLength], etc.)
            this.entidad = new Paises
            {
                Nombre_Pais = "PaisPruebaUnitTest",
                Region = "RegionPrueba"
            };

            // Usamos la capa de aplicaci�n para probar la l�gica de negocio (validaciones, etc.)
            var resultado = this.iAplicacion!.Guardar(this.entidad);

            // Asignamos la entidad resultante (normalmente EF Core llenar� Id)
            this.entidad = resultado;

            return resultado != null && this.entidad!.Id != 0;
        }

        public bool Modificar()
        {
            if (this.entidad == null) return false;

            // Cambiamos un campo para probar la modificaci�n
            this.entidad.Nombre_Pais = "PaisPruebaModificado";

            var resultado = this.iAplicacion!.Modificar(this.entidad);

            // Verificamos que la modificaci�n haya devuelto la entidad y que el nombre haya cambiado
            return resultado != null && resultado.Nombre_Pais == "PaisPruebaModificado";
        }

        public bool Listar()
        {
            // Usamos la capa de aplicaci�n para listar (la implementaci�n toma hasta 20 registros)
            this.lista = this.iAplicacion!.Listar();
            return lista != null && lista.Count > 0;
        }

        public bool Borrar()
        {
            if (this.entidad == null) return false;

            var resultado = this.iAplicacion!.Borrar(this.entidad);

            // Resultado no nulo indica que la l�gica de borrado corri� sin lanzar excepci�n
            // (opcional: comprobar que ya no exista en la DB consultando iConexion)
            return resultado != null;
        }
    }
}
