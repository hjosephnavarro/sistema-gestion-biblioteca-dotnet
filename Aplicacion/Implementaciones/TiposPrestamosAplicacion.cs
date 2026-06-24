using Dominio.Entidades;
using Aplicacion.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repositorio.Interfaces;

namespace Aplicacion.Implementaciones
{
    public class TiposPrestamosAplicacion : ITiposPrestamosAplicacion
    {
        private IConexion? IConexion = null;

        public TiposPrestamosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public TiposPrestamos? Guardar(TiposPrestamos? entidad)
        {
            if (entidad == null) throw new Exception("Falta información del tipo de préstamo");
            if (entidad.Id != 0) throw new Exception("El tipo de préstamo ya existe en la base de datos");

            if (string.IsNullOrWhiteSpace(entidad.Descripcion))
                throw new Exception("La descripción del tipo de préstamo es obligatoria");

            this.IConexion!.TiposPrestamos!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public TiposPrestamos? Modificar(TiposPrestamos? entidad)
        {
            if (entidad == null) throw new Exception("Falta información del tipo de préstamo");
            if (entidad.Id == 0) throw new Exception("El tipo de préstamo no existe en la base de datos");

            if (string.IsNullOrWhiteSpace(entidad.Descripcion))
                throw new Exception("La descripción del tipo de préstamo es obligatoria");

            var entry = this.IConexion!.Entry(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public TiposPrestamos? Borrar(TiposPrestamos? entidad)
        {
            if (entidad == null) throw new Exception("Falta información del tipo de préstamo");
            if (entidad.Id == 0) throw new Exception("El tipo de préstamo no existe en la base de datos");

            var existente = this.IConexion!.TiposPrestamos?.Find(entidad.Id);
            if (existente == null)
                throw new Exception("El tipo de préstamo indicado no existe en la base de datos");

            this.IConexion!.TiposPrestamos!.Remove(existente);
            this.IConexion.SaveChanges();
            return existente;
        }

        public List<TiposPrestamos> Listar()
        {
            // Ordenamos por descripción para mayor legibilidad
            return this.IConexion!.TiposPrestamos!.OrderBy(tp => tp.Descripcion).Take(20).ToList();
        }
    }
}
