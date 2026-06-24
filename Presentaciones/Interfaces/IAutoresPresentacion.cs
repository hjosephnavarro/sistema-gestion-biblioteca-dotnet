using Dominio.Entidades;

namespace Presentaciones.Interfaces
{
    public  interface IAutoresPresentacion
    {
        Task<List<Autores>> Listar();
        Task<List<Autores>> PorAutores(Autores? entidad);
        Task<Autores?> Guardar(Autores? entidad);
        Task<Autores?> Modificar(Autores? entidad);
        Task<Autores?> Borrar(Autores? entidad);
    }
}
