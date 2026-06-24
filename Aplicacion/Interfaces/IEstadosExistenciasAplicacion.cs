using Dominio.Entidades;

namespace Aplicacion.Interfaces
{
    public interface IEstadosExistenciasAplicacion
    {
        void Configurar(string StringConexion);

        List<EstadosExistencias> Listar();
        EstadosExistencias? Guardar(EstadosExistencias? entidad);
        EstadosExistencias? Modificar(EstadosExistencias? entidad);
        EstadosExistencias? Borrar(EstadosExistencias? entidad);
    }
}
