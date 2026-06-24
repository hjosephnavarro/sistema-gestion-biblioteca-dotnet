using Dominio.Entidades;

namespace Presentaciones.Interfaces
{
    public  interface ITiposPresentacion
    {
        Task<List<Tipos>> Listar();
        Task<List<Tipos>> PorTipos(Tipos? entidad);
        Task<Tipos?> Guardar(Tipos? entidad);
        Task<Tipos?> Modificar(Tipos? entidad);
        Task<Tipos?> Borrar(Tipos? entidad);
    }
}