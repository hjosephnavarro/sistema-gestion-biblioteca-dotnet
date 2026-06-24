using Dominio.Entidades;

namespace Presentaciones.Interfaces
{
    public interface ILibrosAutoresPresentacion
    {
        Task<List<LibrosAutores>> Listar();
        Task<List<LibrosAutores>> PorLibrosAutores(LibrosAutores? entidad);
        Task<LibrosAutores?> Guardar(LibrosAutores? entidad);
        Task<LibrosAutores?> Modificar(LibrosAutores? entidad);
        Task<LibrosAutores?> Borrar(LibrosAutores? entidad);
    }
}
