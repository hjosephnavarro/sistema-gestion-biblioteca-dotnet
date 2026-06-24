using Dominio.Entidades;

namespace Presentaciones.Interfaces
{
    public  interface IPrestamosPresentacion
    {
        Task<List<Prestamos>> Listar();
        Task<List<Prestamos>> PorPrestamos(Prestamos? entidad);
        Task<Prestamos?> Guardar(Prestamos? entidad);
        Task<Prestamos?> Modificar(Prestamos? entidad);
        Task<Prestamos?> Borrar(Prestamos? entidad);
    }
}