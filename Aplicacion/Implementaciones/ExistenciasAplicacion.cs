using Dominio.Entidades;
using Aplicacion.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repositorio.Interfaces;

namespace Aplicacion.Implementaciones
{
    public class ExistenciasAplicacion : IExistenciasAplicacion
    {
        private IConexion? IConexion = null;

        public ExistenciasAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Existencias? Guardar(Existencias? entidad)
        {
            if (entidad == null) throw new Exception("Falta información");
            if (entidad.Id != 0) throw new Exception("La existencia ya se encuentra registrada");

            this.IConexion!.Existencias!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Existencias? Modificar(Existencias? entidad)
        {
            if (entidad == null) throw new Exception("Falta información");
            if (entidad.Id == 0) throw new Exception("La existencia no existe en la base de datos");

            var entry = this.IConexion!.Entry(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Existencias? Borrar(Existencias? entidad)
        {
            if (entidad == null) throw new Exception("Falta información");
            if (entidad.Id == 0) throw new Exception("La existencia no existe en la base de datos");

            this.IConexion!.Existencias!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Existencias> Listar()
        {
            return this.IConexion!.Existencias!.Take(20).ToList();
        }
    }
}
