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
    public class PrestamosPrueba
    {
        private readonly IPrestamosAplicacion? iAplicacion;
        private readonly IConexion? iConexion;
        private List<Prestamos>? lista;
        private Prestamos? entidad;

        // Dependencias para FK
        private Usuarios? usuarioTest;
        private TiposPrestamos? tipoPrestamoTest;
        private Editoriales? editorialTest;
        private Paises? paisTest;
        private Tipos? tipoLibroTest;
        private Libros? libroTest;
        private Existencias? existenciaTest;

        public PrestamosPrueba()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            iAplicacion = new PrestamosAplicacion(iConexion);
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

        public bool PrepararDependencias()
        {
            // Usuario
            this.usuarioTest = EntidadesNucleo.Usuarios() ?? new Usuarios();
            this.iConexion!.Usuarios!.Add(this.usuarioTest);
            this.iConexion.SaveChanges();

            // Tipo de préstamo
            this.tipoPrestamoTest = EntidadesNucleo.TiposPrestamos() ?? new TiposPrestamos();
            this.iConexion.TiposPrestamos!.Add(this.tipoPrestamoTest);
            this.iConexion.SaveChanges();

            // Editorial
            this.editorialTest = EntidadesNucleo.Editoriales() ?? new Editoriales();
            this.iConexion.Editoriales!.Add(this.editorialTest);
            this.iConexion.SaveChanges();

            // País
            this.paisTest = EntidadesNucleo.Paises() ?? new Paises();
            this.iConexion.Paises!.Add(this.paisTest);
            this.iConexion.SaveChanges();

            // Tipo de libro
            this.tipoLibroTest = EntidadesNucleo.Tipos() ?? new Tipos();
            this.iConexion.Tipos!.Add(this.tipoLibroTest);
            this.iConexion.SaveChanges();

            // Libro (requiere Editorial, Pais y Tipo)
            this.libroTest = EntidadesNucleo.Libros(this.editorialTest, this.paisTest, this.tipoLibroTest) ?? new Libros();
            this.iConexion.Libros!.Add(this.libroTest);
            this.iConexion.SaveChanges();

            // Existencia (requiere libro)
            this.existenciaTest = EntidadesNucleo.Existencias(this.libroTest) ?? new Existencias();
            this.iConexion.Existencias!.Add(this.existenciaTest);
            this.iConexion.SaveChanges();

            // Verificamos IDs
            return usuarioTest.Id != 0 && tipoPrestamoTest.Id != 0 &&
                   editorialTest.Id != 0 && paisTest.Id != 0 &&
                   tipoLibroTest.Id != 0 && libroTest.Id != 0 &&
                   existenciaTest.Id != 0;
        }

        public bool Guardar()
        {
            this.entidad = new Prestamos
            {
                Usuario = this.usuarioTest!.Id,
                Tipo_Prestamo = this.tipoPrestamoTest!.Id,
                Existencia = this.existenciaTest!.Id,
                Fecha_Prestamo = DateTime.UtcNow.AddMinutes(-5),
                Fecha_Devolucion = DateTime.UtcNow.AddDays(7)
            };

            var resultado = this.iAplicacion!.Guardar(this.entidad);
            this.entidad = resultado;
            return resultado != null && this.entidad!.Id != 0;
        }

        public bool Modificar()
        {
            if (this.entidad == null) return false;

            this.entidad.Fecha_Devolucion = this.entidad.Fecha_Prestamo.AddDays(14);
            var resultado = this.iAplicacion!.Modificar(this.entidad);
            return resultado != null && resultado.Fecha_Devolucion == this.entidad.Fecha_Devolucion;
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
