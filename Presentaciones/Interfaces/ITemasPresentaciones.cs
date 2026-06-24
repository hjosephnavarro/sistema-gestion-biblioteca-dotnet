using Dominio.Entidades;

namespace Presentaciones.Interfaces
{
    public  interface ITemasPresentacion
    {
        Task<List<Temas>> Listar();
        Task<List<Temas>> PorTemas(Temas? entidad);
        Task<Temas?> Guardar(Temas? entidad);
        Task<Temas?> Modificar(Temas? entidad);
        Task<Temas?> Borrar(Temas? entidad);
    }
}