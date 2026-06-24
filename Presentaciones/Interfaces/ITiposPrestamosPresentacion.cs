using Dominio.Entidades;

namespace Presentaciones.Interfaces
{
    public  interface ITiposPrestamosPresentacion
    {
        Task<List<TiposPrestamos>> Listar();
        Task<List<TiposPrestamos>> PorTiposPrestamos(TiposPrestamos? entidad);
        Task<TiposPrestamos?> Guardar(TiposPrestamos? entidad);
        Task<TiposPrestamos?> Modificar(TiposPrestamos? entidad);
        Task<TiposPrestamos?> Borrar(TiposPrestamos? entidad);
    }
}