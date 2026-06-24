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
    public class LibrosTemasPrueba
    {
        private readonly ILibrosTemasAplicacion? iAplicacion;
        private readonly IConexion? iConexion;
        private List<LibrosTemas>? lista;
        private LibrosTemas? entidad;

        public LibrosTemasPrueba()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            iAplicacion = new LibrosTemasAplicacion(iConexion);
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
            // Crear Editorial mínima si no existe
            var editorial = iConexion!.Editoriales!.FirstOrDefault()
                            ?? new Editoriales { Nombre_Editorial = "Editorial prueba" };
            if (editorial.Id == 0)
            {
                iConexion.Editoriales!.Add(editorial);
                iConexion.SaveChanges();
            }

            // Crear País mínimo si no existe
            var pais = iConexion.Paises!.FirstOrDefault()
                       ?? new Paises { Nombre_Pais = "País prueba" };
            if (pais.Id == 0)
            {
                iConexion.Paises!.Add(pais);
                iConexion.SaveChanges();
            }

            // Crear Tipo mínimo si no existe
            var tipo = iConexion.Tipos!.FirstOrDefault()
                       ?? new Tipos { Nombre_Tipo = "Tipo prueba" };
            if (tipo.Id == 0)
            {
                iConexion.Tipos!.Add(tipo);
                iConexion.SaveChanges();
            }

            // Crear libro y asegurarnos de que se guarde en la DB
            var libro = EntidadesNucleo.Libros(editorial, pais, tipo);
            iConexion.Libros!.Add(libro);
            iConexion.SaveChanges(); // 🔹 Fundamental para que libro.Id tenga valor real

            // Crear tema mínimo si no existe
            var tema = iConexion.Temas!.FirstOrDefault() ?? EntidadesNucleo.Temas();
            if (tema.Id == 0)
            {
                iConexion.Temas!.Add(tema);
                iConexion.SaveChanges();
            }

            // Crear relación libro-tema
            this.entidad = new LibrosTemas
            {
                Libro = libro.Id,
                Tema = tema.Id
            };

            var resultado = this.iAplicacion!.Guardar(this.entidad);
            this.entidad = resultado;
            return resultado != null && this.entidad!.Id != 0;
        }

        public bool Modificar()
        {
            if (this.entidad == null) return false;

            // Para modificar, se puede cambiar el tema asociado
            var nuevoTema = iConexion!.Temas!.FirstOrDefault(t => t.Id != this.entidad.Tema) ?? this.entidad.TemaNavigation;
            if (nuevoTema != null)
                this.entidad.Tema = nuevoTema.Id;

            var resultado = this.iAplicacion!.Modificar(this.entidad);
            return resultado != null && resultado.Tema == this.entidad.Tema;
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
