using Dominio.Entidades;

namespace Presentaciones.Interfaces
{
    public  interface ILibrosTemasPresentacion
    {
        Task<List<LibrosTemas>> Listar();
        Task<List<LibrosTemas>> PorLibrosTemas(LibrosTemas? entidad);
        Task<LibrosTemas?> Guardar(LibrosTemas? entidad);
        Task<LibrosTemas?> Modificar(LibrosTemas? entidad);
        Task<LibrosTemas?> Borrar(LibrosTemas? entidad);
    }
}