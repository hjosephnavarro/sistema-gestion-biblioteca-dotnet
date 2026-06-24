using Dominio.Entidades;
using System.Collections.Generic;

namespace Aplicacion.Interfaces
{
    public interface ITiposAplicacion
    {
        void Configurar(string StringConexion);

        List<Tipos> Listar();
        Tipos? Guardar(Tipos? entidad);
        Tipos? Modificar(Tipos? entidad);
        Tipos? Borrar(Tipos? entidad);
    }
}
