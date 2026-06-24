using Dominio.Entidades;
using Presentaciones.Interfaces;

namespace Presentaciones.Implementaciones
{
    public class SancionesPresentacion : ISancionesPresentacion
    {
        private static List<Sanciones> _sancionesLocal = new List<Sanciones>();
        private static int _nextId = 1;

        public async Task<List<Sanciones>> Listar()
        {
            return await Task.FromResult(_sancionesLocal);
        }

        public async Task<List<Sanciones>> PorSanciones(Sanciones? entidad)
        {
            if (entidad == null) 
                return await Task.FromResult(_sancionesLocal);
            
            var resultado = _sancionesLocal.Where(s => 
                (entidad.Id == 0 || s.Id == entidad.Id) &&
                (entidad.Usuario == 0 || s.Usuario == entidad.Usuario)
            ).ToList();
            
            return await Task.FromResult(resultado);
        }

        public async Task<Sanciones?> Guardar(Sanciones? entidad)
        {
            if (entidad == null) 
                return null;
            
            if (entidad.Id != 0) 
                throw new Exception("lbFaltaInformacion");

            entidad.Id = _nextId++;
            _sancionesLocal.Add(entidad);
            
            return await Task.FromResult(entidad);
        }

        public async Task<Sanciones?> Modificar(Sanciones? entidad)
        {
            if (entidad == null) 
                return null;
            
            if (entidad.Id == 0) 
                throw new Exception("lbFaltaInformacion");

            var existente = _sancionesLocal.FirstOrDefault(s => s.Id == entidad.Id);
            if (existente != null)
            {
                existente.Descripcion = entidad.Descripcion;
                existente.Fecha_Inicio = entidad.Fecha_Inicio;
                existente.Fecha_Fin = entidad.Fecha_Fin;
                existente.Usuario = entidad.Usuario;
            }
            
            return await Task.FromResult(entidad);
        }

        public async Task<Sanciones?> Borrar(Sanciones? entidad)
        {
            if (entidad == null) 
                return null;
            
            if (entidad.Id == 0) 
                throw new Exception("lbFaltaInformacion");

            var existente = _sancionesLocal.FirstOrDefault(s => s.Id == entidad.Id);
            if (existente != null)
            {
                _sancionesLocal.Remove(existente);
            }
            
            return await Task.FromResult(entidad);
        }
    }
}