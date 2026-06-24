using Dominio.Entidades;

namespace Presentaciones.Interfaces
{
    public  interface IEstadosPresentacion
    {
        Task<List<Estados>> Listar();
        Task<List<Estados>> PorEstados(Estados? entidad);
        Task<Estados?> Guardar(Estados? entidad);
        Task<Estados?> Modificar(Estados? entidad);
        Task<Estados?> Borrar(Estados? entidad);
    }
}
