using Dominio.Entidades;

namespace Aplicacion.Interfaces
{
    public interface IEditorialesAplicacion
    {
        void Configurar(string StringConexion);
        List<Editoriales> Listar();
        Editoriales? Guardar(Editoriales? entidad);
        Editoriales? Modificar(Editoriales? entidad);
        Editoriales? Borrar(Editoriales? entidad);
    }
}
