using Dominio.Entidades;

namespace Aplicacion.Interfaces
{
    public interface IPrestamosAplicacion
    {
        void Configurar(string StringConexion);

        List<Prestamos> Listar();
        Prestamos? Guardar(Prestamos? entidad);
        Prestamos? Modificar(Prestamos? entidad);
        Prestamos? Borrar(Prestamos? entidad);

        // Nuevo método
        Prestamos SolicitarPrestamo(int usuarioId, int libroId, int cantidad);
    }
}
