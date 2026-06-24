using Dominio.Entidades;
using Aplicacion.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repositorio.Interfaces;

namespace Aplicacion.Implementaciones
{
    public class LibrosTemasAplicacion : ILibrosTemasAplicacion
    {
        private IConexion? IConexion = null;

        public LibrosTemasAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public LibrosTemas? Guardar(LibrosTemas? entidad)
        {
            if (entidad == null)
                throw new Exception("Falta información del registro");

            if (entidad.Id != 0)
                throw new Exception("El registro ya existe");

            // Validar existencia de libro y tema
            if (!this.IConexion!.Libros!.Any(l => l.Id == entidad.Libro))
                throw new Exception("El libro no existe en la base de datos");

            if (!this.IConexion!.Temas!.Any(t => t.Id == entidad.Tema))
                throw new Exception("El tema no existe en la base de datos");

            // Evitar duplicados de la misma combinación libro-tema
            bool existe = this.IConexion!.LibrosTemas!
                .Any(lt => lt.Libro == entidad.Libro && lt.Tema == entidad.Tema);
            if (existe)
                throw new Exception("La relación libro-tema ya está registrada");

            this.IConexion.LibrosTemas!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public LibrosTemas? Modificar(LibrosTemas? entidad)
        {
            if (entidad == null)
                throw new Exception("Falta información del registro");

            if (entidad.Id == 0)
                throw new Exception("El registro no existe en la base de datos");

            // Validar existencia de libro y tema
            if (!this.IConexion!.Libros!.Any(l => l.Id == entidad.Libro))
                throw new Exception("El libro no existe en la base de datos");

            if (!this.IConexion!.Temas!.Any(t => t.Id == entidad.Tema))
                throw new Exception("El tema no existe en la base de datos");

            // Validar duplicado en modificación
            bool duplicado = this.IConexion!.LibrosTemas!
                .Any(lt => lt.Libro == entidad.Libro && lt.Tema == entidad.Tema && lt.Id != entidad.Id);
            if (duplicado)
                throw new Exception("Ya existe otro registro con el mismo libro y tema");

            var entry = this.IConexion.Entry(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public LibrosTemas? Borrar(LibrosTemas? entidad)
        {
            if (entidad == null)
                throw new Exception("Falta información del registro");

            if (entidad.Id == 0)
                throw new Exception("El registro no existe en la base de datos");

            var existente = this.IConexion!.LibrosTemas!.Find(entidad.Id);
            if (existente == null)
                throw new Exception("No se encontró el registro para eliminar");

            this.IConexion.LibrosTemas!.Remove(existente);
            this.IConexion.SaveChanges();
            return existente;
        }

        public List<LibrosTemas> Listar()
        {
            return this.IConexion!.LibrosTemas!
                .Include(lt => lt.LibroNavigation)
                .Include(lt => lt.TemaNavigation)
                .OrderBy(lt => lt.Id)
                .Take(20)
                .ToList();
        }
    }
}
