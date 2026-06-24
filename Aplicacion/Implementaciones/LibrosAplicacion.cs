using Dominio.Entidades;
using Aplicacion.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repositorio.Interfaces;

namespace Aplicacion.Implementaciones
{
    public class LibrosAplicacion : ILibrosAplicacion
    {
        private readonly IConexion? IConexion;

        public LibrosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Libros? Guardar(Libros? entidad)
        {
            // --- Validaciones generales ---
            if (entidad == null) throw new Exception("Falta información del libro");
            if (entidad.Id != 0) throw new Exception("El libro ya tiene un identificador, no se puede volver a registrar");
            
            // --- Validaciones de datos requeridos ---
            if (string.IsNullOrWhiteSpace(entidad.Isbn)) throw new Exception("El ISBN es obligatorio");
            if (string.IsNullOrWhiteSpace(entidad.Titulo)) throw new Exception("El título del libro es obligatorio");
            if (entidad.Editorial <= 0) throw new Exception("Debe especificar una editorial válida");
            if (entidad.Pais <= 0) throw new Exception("Debe especificar un país válido");
            if (entidad.Tipo <= 0) throw new Exception("Debe especificar un tipo de libro válido");

            // --- Validaciones de formato ---
            if (entidad.Isbn.Length > 20) throw new Exception("El ISBN no puede exceder los 20 caracteres");
            if (entidad.Titulo.Length > 200) throw new Exception("El título no puede exceder los 200 caracteres");
            if (entidad.Edicion != null && entidad.Edicion.Length > 50)
                throw new Exception("La edición no puede exceder los 50 caracteres");

            // --- Validación de duplicados ---
            bool existeIsbn = this.IConexion!.Libros!.Any(l => l.Isbn == entidad.Isbn);
            if (existeIsbn)
                throw new Exception($"Ya existe un libro registrado con el ISBN {entidad.Isbn}");

            // --- Validación de existencia de claves foráneas ---
            if (!this.IConexion!.Editoriales!.Any(e => e.Id == entidad.Editorial))
                throw new Exception("La editorial especificada no existe");
            if (!this.IConexion.Paises!.Any(p => p.Id == entidad.Pais))
                throw new Exception("El país especificado no existe");
            if (!this.IConexion.Tipos!.Any(t => t.Id == entidad.Tipo))
                throw new Exception("El tipo especificado no existe");

            // --- Registro ---
            this.IConexion.Libros.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Libros? Modificar(Libros? entidad)
        {
            if (entidad == null) throw new Exception("Falta información del libro");
            if (entidad.Id == 0) throw new Exception("Debe especificar el identificador del libro a modificar");

            var libroExistente = this.IConexion!.Libros!.Find(entidad.Id);
            if (libroExistente == null)
                throw new Exception("El libro no existe en la base de datos");

            // --- Validaciones de actualización ---
            if (string.IsNullOrWhiteSpace(entidad.Titulo))
                throw new Exception("El título del libro no puede quedar vacío");
            if (string.IsNullOrWhiteSpace(entidad.Isbn))
                throw new Exception("El ISBN es obligatorio");

            // --- Evitar duplicar ISBN en otro libro ---
            bool isbnDuplicado = this.IConexion.Libros.Any(l => l.Isbn == entidad.Isbn && l.Id != entidad.Id);
            if (isbnDuplicado)
                throw new Exception($"Ya existe otro libro con el ISBN {entidad.Isbn}");

            // --- Actualización segura ---
            this.IConexion.Entry(libroExistente).CurrentValues.SetValues(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Libros? Borrar(Libros? entidad)
        {
            if (entidad == null) throw new Exception("Falta información del libro");
            if (entidad.Id == 0) throw new Exception("Debe especificar el identificador del libro a eliminar");

            var libro = this.IConexion!.Libros!.Include(l => l.Existencias)
                                              .Include(l => l.LibrosAutores)
                                              .Include(l => l.LibrosTemas)
                                              .FirstOrDefault(l => l.Id == entidad.Id);

            if (libro == null)
                throw new Exception("El libro no existe en la base de datos");

            // --- Validación de dependencias ---
            if (libro.Existencias.Any())
                throw new Exception("No se puede eliminar el libro porque tiene existencias registradas");
            if (libro.LibrosAutores.Any())
                throw new Exception("No se puede eliminar el libro porque tiene autores asociados");
            if (libro.LibrosTemas.Any())
                throw new Exception("No se puede eliminar el libro porque tiene temas asociados");

            // --- Eliminación ---
            this.IConexion.Libros.Remove(libro);
            this.IConexion.SaveChanges();
            return libro;
        }

        public List<Libros> Listar()
        {
            return this.IConexion!.Libros!
                .Include(l => l.EditorialNavigation)
                .Include(l => l.PaisNavigation)
                .Include(l => l.LibrosAutores)
                    .ThenInclude(la => la.AutorNavigation)
                .Include(l => l.LibrosTemas)
                    .ThenInclude(lt => lt.TemaNavigation)
                .OrderBy(l => l.Titulo)
                .ToList();
        }
    }
}
