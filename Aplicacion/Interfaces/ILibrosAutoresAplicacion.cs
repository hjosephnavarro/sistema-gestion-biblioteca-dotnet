using Dominio.Entidades;

namespace Aplicacion.Interfaces
{
    public interface ILibrosAutoresAplicacion
    {
        void Configurar(string StringConexion);

        List<LibrosAutores> Listar();
        LibrosAutores? Guardar(LibrosAutores? entidad);
        LibrosAutores? Modificar(LibrosAutores? entidad);
        LibrosAutores? Borrar(LibrosAutores? entidad);
    }
}
