using Dominio.Entidades;

namespace Presentaciones.Interfaces
{
    public  interface ISancionesPresentacion
    {
        Task<List<Sanciones>> Listar();
        Task<List<Sanciones>> PorSanciones(Sanciones? entidad);
        Task<Sanciones?> Guardar(Sanciones? entidad);
        Task<Sanciones?> Modificar(Sanciones? entidad);
        Task<Sanciones?> Borrar(Sanciones? entidad);
    }
}