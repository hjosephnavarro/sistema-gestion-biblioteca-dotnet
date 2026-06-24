using Dominio.Entidades;
using Aplicacion.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repositorio.Interfaces;

namespace Aplicacion.Implementaciones
{
    public class AutoresAplicacion : IAutoresAplicacion
    {
        private IConexion? IConexion = null;

        public AutoresAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Autores? Guardar(Autores? entidad)
        {
            if (entidad == null)
                throw new Exception("Falta información del autor.");

            if (entidad.Id != 0)
                throw new Exception("El autor ya se encuentra registrado.");

            // Verificar duplicados por nombre (opcional según modelo)
            bool existe = this.IConexion!.Autores!.Any(a => a.Nombre == entidad.Nombre);
            if (existe)
                throw new Exception("Ya existe un autor con ese nombre.");

            this.IConexion.Autores!.Add(entidad);
            this.IConexion.SaveChanges();

            return entidad;
        }

        public Autores? Modificar(Autores? entidad)
        {
            if (entidad == null)
                throw new Exception("Falta información del autor.");

            if (entidad.Id == 0)
                throw new Exception("El autor no existe en la base de datos.");

            var original = this.IConexion!.Autores!.FirstOrDefault(a => a.Id == entidad.Id);
            if (original == null)
                throw new Exception("No se encontró el autor para modificar.");

            // Actualización de campos principales
            original.Nombre = entidad.Nombre;
            original.Nacionalidad = entidad.Nacionalidad;

            this.IConexion.Entry(original).State = EntityState.Modified;
            this.IConexion.SaveChanges();

            return original;
        }

        public Autores? Borrar(Autores? entidad)
        {
            if (entidad == null)
                throw new Exception("Falta información del autor.");

            if (entidad.Id == 0)
                throw new Exception("El autor no existe en la base de datos.");

            var autor = this.IConexion!.Autores!.FirstOrDefault(a => a.Id == entidad.Id);
            if (autor == null)
                throw new Exception("No se encontró el autor a eliminar.");

            this.IConexion.Autores!.Remove(autor);
            this.IConexion.SaveChanges();

            return autor;
        }

        public List<Autores> Listar()
        {
            return this.IConexion!.Autores!
                .Include(a => a.LibrosAutores)
                    .ThenInclude(la => la.LibroNavigation)
                .OrderBy(a => a.Nombre)
                .ToList();
        }
    }
}
