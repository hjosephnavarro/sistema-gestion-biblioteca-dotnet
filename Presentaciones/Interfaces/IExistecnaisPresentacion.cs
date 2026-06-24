using Dominio.Entidades;

namespace Presentaciones.Interfaces
{
    public  interface IExistenciasPresentacion
    {
        Task<List<Existencias>> Listar();
        Task<List<Existencias>> PorExistencias(Existencias? entidad);
        Task<Existencias?> Guardar(Existencias? entidad);
        Task<Existencias?> Modificar(Existencias? entidad);
        Task<Existencias?> Borrar(Existencias? entidad);
    }
}