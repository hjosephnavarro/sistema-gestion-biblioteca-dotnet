using Dominio.Nucleo;
using Dominio.Entidades;
using Presentaciones.Interfaces;

namespace Presentaciones.Implementaciones
{
    public class SancionesPresentacion : ISancionesPresentacion
    {
        private Comunicaciones? comunicaciones = null;
        private readonly AuditoriasPresentacion auditor = new AuditoriasPresentacion();

        public async Task<List<Sanciones>> Listar()
        {
            var lista = new List<Sanciones>();
            var datos = new Dictionary<string, object>();

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Sanciones/Listar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            lista = JsonConversor.ConvertirAObjeto<List<Sanciones>>(
                JsonConversor.ConvertirAString(respuesta["Entidades"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Sanciones",
                Accion = "Listar",
                UsuarioAccion = "Sistema",
                DatosAntes = null,
                DatosDespues = $"Se listaron {lista.Count} Sanciones.",
                FechaAccion = DateTime.Now
            });

            return lista;
        }

        public async Task<List<Sanciones>> PorSanciones(Sanciones? entidad)
        {
            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad!;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Sanciones/PorSanciones");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            var lista = JsonConversor.ConvertirAObjeto<List<Sanciones>>(
                JsonConversor.ConvertirAString(respuesta["Entidades"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Sanciones",
                Accion = "Consulta por usuario",
                UsuarioAccion = "Sistema",
                DatosAntes = JsonConversor.ConvertirAString(entidad),
                DatosDespues = $"Resultado: {lista.Count} registros.",
                FechaAccion = DateTime.Now
            });

            return lista;
        }

        public async Task<Sanciones?> Guardar(Sanciones? entidad)
        {
            if (entidad!.Id != 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Sanciones/Guardar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            entidad = JsonConversor.ConvertirAObjeto<Sanciones>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Sanciones",
                EntidadId = entidad.Id,
                Accion = "Guardar",
                UsuarioAccion = "Sistema",
                DatosAntes = null,
                DatosDespues = JsonConversor.ConvertirAString(entidad),
                FechaAccion = DateTime.Now
            });

            return entidad;
        }

        public async Task<Sanciones?> Modificar(Sanciones? entidad)
        {
            if (entidad!.Id == 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Sanciones/Modificar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            var entidadNueva = JsonConversor.ConvertirAObjeto<Sanciones>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Sanciones",
                EntidadId = entidadNueva.Id,
                Accion = "Modificar",
                UsuarioAccion = "Sistema",
                DatosAntes = JsonConversor.ConvertirAString(entidad),
                DatosDespues = JsonConversor.ConvertirAString(entidadNueva),
                FechaAccion = DateTime.Now
            });

            return entidadNueva;
        }

        public async Task<Sanciones?> Borrar(Sanciones? entidad)
        {
            if (entidad!.Id == 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Sanciones/Borrar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            var resultado = JsonConversor.ConvertirAObjeto<Sanciones>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Sanciones",
                EntidadId = resultado.Id,
                Accion = "Borrar",
                UsuarioAccion = "Sistema",
                DatosAntes = JsonConversor.ConvertirAString(entidad),
                DatosDespues = null,
                FechaAccion = DateTime.Now
            });

            return resultado;
        }
    }
}