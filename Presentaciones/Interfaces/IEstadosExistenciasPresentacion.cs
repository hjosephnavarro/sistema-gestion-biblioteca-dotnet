using Dominio.Entidades;

namespace Presentaciones.Interfaces
{
    public interface IEstadosExistenciasPresentacion
    {
        Task<List<EstadosExistencias>> Listar();
        Task<List<EstadosExistencias>> PorEstadosExistencias(EstadosExistencias? entidad);
        Task<EstadosExistencias?> Guardar(EstadosExistencias? entidad);
        Task<EstadosExistencias?> Modificar(EstadosExistencias? entidad);
        Task<EstadosExistencias?> Borrar(EstadosExistencias? entidad);
    }
}
