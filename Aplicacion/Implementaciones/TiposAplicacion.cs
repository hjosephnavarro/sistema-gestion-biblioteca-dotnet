using Dominio.Entidades;
using Aplicacion.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repositorio.Interfaces;
namespace Aplicacion.Implementaciones
{
    public class TiposAplicacion : ITiposAplicacion
    {
        private IConexion? IConexion = null;

        public TiposAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Tipos? Guardar(Tipos? entidad)
        {
            if (entidad == null) throw new Exception("Falta información del tipo");
            if (entidad.Id != 0) throw new Exception("El tipo ya existe en la base de datos");

            if (string.IsNullOrWhiteSpace(entidad.Nombre_Tipo))
                throw new Exception("El nombre del tipo es obligatorio");

            this.IConexion!.Tipos!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Tipos? Modificar(Tipos? entidad)
        {
            if (entidad == null) throw new Exception("Falta información del tipo");
            if (entidad.Id == 0) throw new Exception("El tipo no existe en la base de datos");

            if (string.IsNullOrWhiteSpace(entidad.Nombre_Tipo))
                throw new Exception("El nombre del tipo es obligatorio");

            var entry = this.IConexion!.Entry(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Tipos? Borrar(Tipos? entidad)
        {
            if (entidad == null) throw new Exception("Falta información del tipo");
            if (entidad.Id == 0) throw new Exception("El tipo no existe en la base de datos");

            var existente = this.IConexion!.Tipos?.Find(entidad.Id);
            if (existente == null)
                throw new Exception("El tipo indicado no existe en la base de datos");

            this.IConexion!.Tipos!.Remove(existente);
            this.IConexion.SaveChanges();
            return existente;
        }

        public List<Tipos> Listar()
        {
            // Ordenamos por nombre para mayor legibilidad
            return this.IConexion!.Tipos!.OrderBy(t => t.Nombre_Tipo).Take(20).ToList();
        }
    }
}
