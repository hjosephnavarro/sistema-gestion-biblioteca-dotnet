using Dominio.Entidades;
using Aplicacion.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repositorio.Interfaces;

namespace Aplicacion.Implementaciones
{
    public class EditorialesAplicacion : IEditorialesAplicacion
    {
        private IConexion? IConexion = null;

        public EditorialesAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Editoriales? Guardar(Editoriales? entidad)
        {
            if (entidad == null)
                throw new Exception("Falta información");
            if (entidad.Id != 0)
                throw new Exception("La editorial ya se encuentra registrada");

            this.IConexion!.Editoriales!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Editoriales? Modificar(Editoriales? entidad)
        {
            if (entidad == null)
                throw new Exception("Falta información");
            if (entidad.Id == 0)
                throw new Exception("La editorial no existe en la base de datos");

            var entry = this.IConexion!.Entry<Editoriales>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Editoriales? Borrar(Editoriales? entidad)
        {
            if (entidad == null)
                throw new Exception("Falta información");
            if (entidad.Id == 0)
                throw new Exception("La editorial no existe en la base de datos");

            this.IConexion!.Editoriales!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Editoriales> Listar()
        {
            return this.IConexion!.Editoriales!.Take(20).ToList();
        }
    }
}
