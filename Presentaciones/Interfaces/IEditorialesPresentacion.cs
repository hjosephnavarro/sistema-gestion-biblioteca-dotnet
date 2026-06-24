using Dominio.Entidades;

namespace Presentaciones.Interfaces
{
    public  interface IEditorialesPresentacion
    {
        Task<List<Editoriales>> Listar();
        Task<List<Editoriales>> PorEditoriales(Editoriales? entidad);
        Task<Editoriales?> Guardar(Editoriales? entidad);
        Task<Editoriales?> Modificar(Editoriales? entidad);
        Task<Editoriales?> Borrar(Editoriales? entidad);
    }
}
