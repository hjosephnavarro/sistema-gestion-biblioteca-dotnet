using Dominio.Entidades;
using Repositorio.Implementaciones;
using Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class EstadosExistenciasPrueba
    {
        private readonly IConexion? iConexion;
        private List<EstadosExistencias>? lista;
        private EstadosExistencias? entidad;

        public EstadosExistenciasPrueba()
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
            // Crear dependencias
            var editorial = EntidadesNucleo.Editoriales()!;
            var pais = EntidadesNucleo.Paises()!;
            var tipo = EntidadesNucleo.Tipos()!;

            this.iConexion!.Editoriales!.Add(editorial);
            this.iConexion!.Paises!.Add(pais);
            this.iConexion!.Tipos!.Add(tipo);
            this.iConexion!.SaveChanges();

            var libro = EntidadesNucleo.Libros(editorial, pais, tipo)!;
            this.iConexion!.Libros!.Add(libro);
            this.iConexion!.SaveChanges();

            var existencia = EntidadesNucleo.Existencias(libro)!;
            this.iConexion!.Existencias!.Add(existencia);
            this.iConexion!.SaveChanges();

            var estado = EntidadesNucleo.Estados()!;
            this.iConexion!.Estados!.Add(estado);
            this.iConexion!.SaveChanges();

            // Crear la entidad principal
            this.entidad = EntidadesNucleo.EstadosExistencias(existencia, estado)!;
            this.iConexion!.EstadosExistencias!.Add(this.entidad);
            this.iConexion!.SaveChanges();

            return true;
        }

        public bool Modificar()
        {
            // Cambiar el estado asociado
            var nuevoEstado = EntidadesNucleo.Estados()!;
            nuevoEstado.Nombre_Estado = "Estado Modificado";
            this.iConexion!.Estados!.Add(nuevoEstado);
            this.iConexion!.SaveChanges();

            this.entidad!.Estado = nuevoEstado.Id;
            var entry = this.iConexion!.Entry<EstadosExistencias>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();

            return true;
        }

        public bool Listar()
        {
            this.lista = this.iConexion!.EstadosExistencias!.ToList();
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
