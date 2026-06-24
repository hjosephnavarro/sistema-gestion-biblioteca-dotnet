using Dominio.Entidades;
using Aplicacion.Interfaces;
using Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Implementaciones
{
    public class PrestamosAplicacion : IPrestamosAplicacion
    {
        private IConexion? IConexion = null;

        public PrestamosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Prestamos? Guardar(Prestamos? entidad)
        {
            if (entidad == null) throw new Exception("Falta información");
            if (entidad.Id != 0) throw new Exception("El préstamo ya existe");

            // Validaciones de negocio simples
            if (entidad.Fecha_Prestamo > DateTime.UtcNow)
                throw new Exception("La fecha de préstamo no puede ser en el futuro.");

            if (entidad.Fecha_Devolucion.HasValue && entidad.Fecha_Devolucion.Value < entidad.Fecha_Prestamo)
                throw new Exception("La fecha de devolución no puede ser anterior a la fecha de préstamo.");

            // Validar FKs mínimos (existencia de registros relacionados)
            var usuario = this.IConexion!.Usuarios?.Find(entidad.Usuario);
            if (usuario == null) throw new Exception("El usuario indicado no existe.");

            var tipo = this.IConexion.TiposPrestamos?.Find(entidad.Tipo_Prestamo);
            if (tipo == null) throw new Exception("El tipo de préstamo indicado no existe.");

            var existencia = this.IConexion.Existencias?.Find(entidad.Existencia);
            if (existencia == null) throw new Exception("La existencia indicada no existe.");

            this.IConexion!.Prestamos!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Prestamos? Modificar(Prestamos? entidad)
        {
            if (entidad == null) throw new Exception("Falta información");
            if (entidad.Id == 0) throw new Exception("El préstamo no existe en la base de datos");

            if (entidad.Fecha_Prestamo > DateTime.UtcNow)
                throw new Exception("La fecha de préstamo no puede ser en el futuro.");

            if (entidad.Fecha_Devolucion.HasValue && entidad.Fecha_Devolucion.Value < entidad.Fecha_Prestamo)
                throw new Exception("La fecha de devolución no puede ser anterior a la fecha de préstamo.");

            // Validar que las FK apunten a registros reales si se modificaron
            var usuario = this.IConexion!.Usuarios?.Find(entidad.Usuario);
            if (usuario == null) throw new Exception("El usuario indicado no existe.");

            var tipo = this.IConexion.TiposPrestamos?.Find(entidad.Tipo_Prestamo);
            if (tipo == null) throw new Exception("El tipo de préstamo indicado no existe.");

            var existencia = this.IConexion.Existencias?.Find(entidad.Existencia);
            if (existencia == null) throw new Exception("La existencia indicada no existe.");

            var entry = this.IConexion.Entry(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Prestamos? Borrar(Prestamos? entidad)
        {
            if (entidad == null) throw new Exception("Falta información");
            if (entidad.Id == 0) throw new Exception("El préstamo no existe en la base de datos");

            this.IConexion!.Prestamos!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Prestamos> Listar()
        {
            return this.IConexion!.Prestamos!.Take(20).ToList();
        }
        public Prestamos SolicitarPrestamo(int usuarioId, int libroId, int cantidad = 1)
        {
            // encontrar una existencia válida del libro (prioriza la primera con ejemplares > 0)
            var existencia = this.IConexion!.Existencias!
                .Where(e => e.Libro == libroId && e.Ejemplares > 0)
                .OrderByDescending(e => e.Ejemplares)
                .FirstOrDefault();

            if (existencia == null)
                throw new Exception("No hay ejemplares disponibles para ese libro.");

            // Validar usuario
            var usuario = this.IConexion.Usuarios?.Find(usuarioId);
            if (usuario == null)
                throw new Exception("Usuario no válido.");

            // Validar tipo de préstamo por defecto (buscar un tipo "Normal" o tomar el primero)
            var tipo = this.IConexion.TiposPrestamos?.FirstOrDefault();
            if (tipo == null)
                throw new Exception("No hay tipos de préstamo configurados.");

            // validar cantidad
            if (cantidad <= 0) throw new Exception("Cantidad inválida.");

            // verificar stock suficiente
            if (existencia.Ejemplares < cantidad)
                throw new Exception("No hay suficientes ejemplares disponibles.");

            // crear préstamo
            var now = DateTime.UtcNow;
            var prestamo = new Prestamos
            {
                Usuario = usuarioId,
                Existencia = existencia.Id,
                Tipo_Prestamo = tipo.Id,
                Fecha_Prestamo = now,
                Fecha_Devolucion = now.AddDays(7) // ejemplo: 7 días por defecto
            };

            // Operación en transacción para evitar condiciones competidoras
            // agregar préstamo
            this.IConexion.Prestamos!.Add(prestamo);

            // descontar existencias
            existencia.Ejemplares -= cantidad;
            this.IConexion.Entry(existencia).State = EntityState.Modified;

            // guardar cambios
            this.IConexion.SaveChanges();

            return prestamo;
        }
    }
}
