using Dominio.Entidades;
using Aplicacion.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repositorio.Interfaces;

namespace Aplicacion.Implementaciones
{
    public class PaisesAplicacion : IPaisesAplicacion
    {
        private IConexion? IConexion = null;

        public PaisesAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Paises? Guardar(Paises? entidad)
        {
            if (entidad == null) throw new Exception("Falta información");
            if (entidad.Id != 0) throw new Exception("El país ya existe");

            this.IConexion!.Paises!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Paises? Modificar(Paises? entidad)
        {
            if (entidad == null) throw new Exception("Falta información");
            if (entidad.Id == 0) throw new Exception("El país no existe en la base de datos");

            var entry = this.IConexion!.Entry(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Paises? Borrar(Paises? entidad)
        {
            if (entidad == null) throw new Exception("Falta información");
            if (entidad.Id == 0) throw new Exception("El país no existe en la base de datos");

            this.IConexion!.Paises!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Paises> Listar()
        {
            return this.IConexion!.Paises!.Take(20).ToList();
        }
    }
}
