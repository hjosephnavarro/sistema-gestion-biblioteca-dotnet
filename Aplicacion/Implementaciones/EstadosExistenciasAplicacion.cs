using Dominio.Entidades;
using Aplicacion.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repositorio.Interfaces;

namespace Aplicacion.Implementaciones
{
    public class EstadosExistenciasAplicacion : IEstadosExistenciasAplicacion
    {
        private IConexion? IConexion = null;

        public EstadosExistenciasAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public EstadosExistencias? Guardar(EstadosExistencias? entidad)
        {
            if (entidad == null) throw new Exception("Falta información");
            if (entidad.Id != 0) throw new Exception("El registro ya existe");

            this.IConexion!.EstadosExistencias!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public EstadosExistencias? Modificar(EstadosExistencias? entidad)
        {
            if (entidad == null) throw new Exception("Falta información");
            if (entidad.Id == 0) throw new Exception("El registro no existe en la base de datos");

            var entry = this.IConexion!.Entry(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public EstadosExistencias? Borrar(EstadosExistencias? entidad)
        {
            if (entidad == null) throw new Exception("Falta información");
            if (entidad.Id == 0) throw new Exception("El registro no existe en la base de datos");

            this.IConexion!.EstadosExistencias!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<EstadosExistencias> Listar()
        {
            return this.IConexion!.EstadosExistencias!.Take(20).ToList();
        }
    }
}
