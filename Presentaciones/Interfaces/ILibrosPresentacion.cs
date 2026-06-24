using Dominio.Entidades;

namespace Presentaciones.Interfaces
{
    public interface ILibrosPresentacion
    {
        Task<List<Libros>> Listar();
        Task<List<Libros>> PorLibros(Libros? entidad);
        Task<Libros?> Guardar(Libros? entidad);
        Task<Libros?> Modificar(Libros? entidad);
        Task<Libros?> Borrar(Libros? entidad);
    }
}
