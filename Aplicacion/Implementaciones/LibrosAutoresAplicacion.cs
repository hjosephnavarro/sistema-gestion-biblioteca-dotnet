using Dominio.Entidades;
using Aplicacion.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repositorio.Interfaces;

namespace Aplicacion.Implementaciones
{
    public class LibrosAutoresAplicacion : ILibrosAutoresAplicacion
    {
        private IConexion? IConexion = null;

        public LibrosAutoresAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public LibrosAutores? Guardar(LibrosAutores? entidad)
        {
            if (entidad == null)
                throw new Exception("Falta información");

            if (entidad.Id != 0)
                throw new Exception("El registro ya existe");

            if (entidad.Libro <= 0)
                throw new Exception("Debe especificar un libro válido");

            if (entidad.Autor <= 0)
                throw new Exception("Debe especificar un autor válido");

            // Validar existencia real de libro y autor
            if (!this.IConexion!.Libros!.Any(l => l.Id == entidad.Libro))
                throw new Exception("El libro especificado no existe en la base de datos");

            if (!this.IConexion!.Autores!.Any(a => a.Id == entidad.Autor))
                throw new Exception("El autor especificado no existe en la base de datos");

            // Evitar duplicados (Libro + Autor)
            if (this.IConexion!.LibrosAutores!.Any(la => la.Libro == entidad.Libro && la.Autor == entidad.Autor))
                throw new Exception("Este autor ya está asociado a este libro");

            this.IConexion!.LibrosAutores!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public LibrosAutores? Modificar(LibrosAutores? entidad)
        {
            if (entidad == null)
                throw new Exception("Falta información");

            if (entidad.Id == 0)
                throw new Exception("El registro no existe en la base de datos");

            if (entidad.Libro <= 0 || entidad.Autor <= 0)
                throw new Exception("Debe especificar libro y autor válidos");

            var existente = this.IConexion!.LibrosAutores!.FirstOrDefault(la => la.Id == entidad.Id);
            if (existente == null)
                throw new Exception("No se encontró el registro para modificar");

            // Validar que no se duplique (Libro + Autor) con otro registro
            if (this.IConexion!.LibrosAutores!.Any(la => la.Libro == entidad.Libro && la.Autor == entidad.Autor && la.Id != entidad.Id))
                throw new Exception("Ya existe una asociación entre este autor y libro");

            var entry = this.IConexion!.Entry(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public LibrosAutores? Borrar(LibrosAutores? entidad)
        {
            if (entidad == null)
                throw new Exception("Falta información");

            if (entidad.Id == 0)
                throw new Exception("El registro no existe en la base de datos");

            var existente = this.IConexion!.LibrosAutores!.FirstOrDefault(la => la.Id == entidad.Id);
            if (existente == null)
                throw new Exception("El registro no existe o ya fue eliminado");

            this.IConexion!.LibrosAutores!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<LibrosAutores> Listar()
        {
            return this.IConexion!.LibrosAutores!
                .Include(la => la.LibroNavigation)
                .Include(la => la.AutorNavigation)
                .Take(20)
                .ToList();
        }
    }
}
