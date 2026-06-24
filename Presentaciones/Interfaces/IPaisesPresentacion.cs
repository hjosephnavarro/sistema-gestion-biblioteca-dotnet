using Dominio.Entidades;

namespace Presentaciones.Interfaces
{
    public  interface IPaisesPresentacion
    {
        Task<List<Paises>> Listar();
        Task<List<Paises>> PorPaises(Paises? entidad);
        Task<Paises?> Guardar(Paises? entidad);
        Task<Paises?> Modificar(Paises? entidad);
        Task<Paises?> Borrar(Paises? entidad);
    }
}