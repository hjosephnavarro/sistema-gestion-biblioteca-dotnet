using Dominio.Entidades;
using Aplicacion.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repositorio.Interfaces;

namespace Aplicacion.Implementaciones
{
    public class SancionesAplicacion : ISancionesAplicacion
    {
        private IConexion? IConexion = null;

        public SancionesAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Sanciones? Guardar(Sanciones? entidad)
        {
            if (entidad == null) throw new Exception("Falta información de la sanción");
            if (entidad.Id != 0) throw new Exception("La sanción ya existe en la base de datos");

            // Validaciones de negocio adicionales
            if (string.IsNullOrWhiteSpace(entidad.Descripcion))
                throw new Exception("La descripción de la sanción no puede estar vacía");

            if (entidad.Fecha_Inicio == default)
                throw new Exception("La fecha de inicio de la sanción es obligatoria");

            if (entidad.Fecha_Fin.HasValue && entidad.Fecha_Fin.Value < entidad.Fecha_Inicio)
                throw new Exception("La fecha de fin no puede ser anterior a la fecha de inicio");

            // Validación de FK Usuario
            var usuario = this.IConexion!.Usuarios?.Find(entidad.Usuario);
            if (usuario == null)
                throw new Exception("El usuario indicado no existe en la base de datos");

            this.IConexion!.Sanciones!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Sanciones? Modificar(Sanciones? entidad)
        {
            if (entidad == null) throw new Exception("Falta información de la sanción");
            if (entidad.Id == 0) throw new Exception("La sanción no existe en la base de datos");

            // Validaciones similares a Guardar
            if (string.IsNullOrWhiteSpace(entidad.Descripcion))
                throw new Exception("La descripción de la sanción no puede estar vacía");

            if (entidad.Fecha_Inicio == default)
                throw new Exception("La fecha de inicio de la sanción es obligatoria");

            if (entidad.Fecha_Fin.HasValue && entidad.Fecha_Fin.Value < entidad.Fecha_Inicio)
                throw new Exception("La fecha de fin no puede ser anterior a la fecha de inicio");

            // Validación de FK Usuario
            var usuario = this.IConexion!.Usuarios?.Find(entidad.Usuario);
            if (usuario == null)
                throw new Exception("El usuario indicado no existe en la base de datos");

            var entry = this.IConexion.Entry(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Sanciones? Borrar(Sanciones? entidad)
        {
            if (entidad == null) throw new Exception("Falta información de la sanción");
            if (entidad.Id == 0) throw new Exception("La sanción no existe en la base de datos");

            // Validación de existencia real
            var existente = this.IConexion!.Sanciones?.Find(entidad.Id);
            if (existente == null)
                throw new Exception("La sanción indicada no existe en la base de datos");

            this.IConexion!.Sanciones!.Remove(existente);
            this.IConexion.SaveChanges();
            return existente;
        }

        public List<Sanciones> Listar()
        {
            // Retorna máximo 20 sanciones, ordenadas por fecha de inicio descendente
            return this.IConexion!.Sanciones!.OrderByDescending(s => s.Fecha_Inicio).Take(20).ToList();
        }
    }
}
