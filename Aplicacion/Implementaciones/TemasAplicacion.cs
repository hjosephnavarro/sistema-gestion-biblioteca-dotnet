using Dominio.Entidades;
using Aplicacion.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repositorio.Interfaces;

namespace Aplicacion.Implementaciones
{
    public class TemasAplicacion : ITemasAplicacion
    {
        private IConexion? IConexion = null;

        public TemasAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Temas? Guardar(Temas? entidad)
        {
            if (entidad == null) throw new Exception("Falta información del tema");
            if (entidad.Id != 0) throw new Exception("El tema ya existe en la base de datos");

            if (string.IsNullOrWhiteSpace(entidad.Nombre_Tema))
                throw new Exception("El nombre del tema es obligatorio");

            this.IConexion!.Temas!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Temas? Modificar(Temas? entidad)
        {
            if (entidad == null) throw new Exception("Falta información del tema");
            if (entidad.Id == 0) throw new Exception("El tema no existe en la base de datos");

            if (string.IsNullOrWhiteSpace(entidad.Nombre_Tema))
                throw new Exception("El nombre del tema es obligatorio");

            var entry = this.IConexion!.Entry(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Temas? Borrar(Temas? entidad)
        {
            if (entidad == null) throw new Exception("Falta información del tema");
            if (entidad.Id == 0) throw new Exception("El tema no existe en la base de datos");

            var existente = this.IConexion!.Temas?.Find(entidad.Id);
            if (existente == null)
                throw new Exception("El tema indicado no existe en la base de datos");

            this.IConexion!.Temas!.Remove(existente);
            this.IConexion.SaveChanges();
            return existente;
        }

        public List<Temas> Listar()
        {
            // Ordenamos por nombre para mejor legibilidad
            return this.IConexion!.Temas!.OrderBy(t => t.Nombre_Tema).Take(20).ToList();
        }
    }
}
