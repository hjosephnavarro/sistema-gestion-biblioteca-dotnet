using Dominio.Entidades;
using Aplicacion.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repositorio.Interfaces;

namespace Aplicacion.Implementaciones
{
    public class EstadosAplicacion : IEstadosAplicacion
    {
        private IConexion? IConexion = null;

        public EstadosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Estados? Guardar(Estados? entidad)
        {
            if (entidad == null) throw new Exception("Falta información");
            if (entidad.Id != 0) throw new Exception("El estado ya existe");

            this.IConexion!.Estados!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Estados? Modificar(Estados? entidad)
        {
            if (entidad == null) throw new Exception("Falta información");
            if (entidad.Id == 0) throw new Exception("El estado no existe en la base de datos");

            var entry = this.IConexion!.Entry(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Estados? Borrar(Estados? entidad)
        {
            if (entidad == null) throw new Exception("Falta información");
            if (entidad.Id == 0) throw new Exception("El estado no existe en la base de datos");

            this.IConexion!.Estados!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Estados> Listar()
        {
            return this.IConexion!.Estados!.Take(20).ToList();
        }
    }
}
