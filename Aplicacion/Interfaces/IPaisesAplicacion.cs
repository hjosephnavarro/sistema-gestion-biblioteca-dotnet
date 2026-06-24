using Dominio.Entidades;

namespace Aplicacion.Interfaces
{
    public interface IPaisesAplicacion
    {
        void Configurar(string StringConexion);

        List<Paises> Listar();
        Paises? Guardar(Paises? entidad);
        Paises? Modificar(Paises? entidad);
        Paises? Borrar(Paises? entidad);
    }
}
