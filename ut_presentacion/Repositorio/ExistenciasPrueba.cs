using Dominio.Entidades;
using Repositorio.Implementaciones;
using Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class ExistenciasPrueba
    {
        private readonly IConexion? iConexion;
        private List<Existencias>? lista;
        private Existencias? entidad;

        public ExistenciasPrueba()
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
            // Crear dependencias necesarias para Existencias
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

            // Ahora sí crear la existencia
            this.entidad = EntidadesNucleo.Existencias(libro)!;
            this.iConexion!.Existencias!.Add(this.entidad);
            this.iConexion!.SaveChanges();

            return true;
        }

        public bool Modificar()
        {
            this.entidad!.Ejemplares = 10; // modificamos cantidad
            var entry = this.iConexion!.Entry<Existencias>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();

            return true;
        }

        public bool Listar()
        {
            this.lista = this.iConexion!.Existencias!.ToList();
            return lista.Count > 0;
        }

        public bool Borrar()
        {
            this.iConexion!.Existencias!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();

            return true;
        }
    }
}
