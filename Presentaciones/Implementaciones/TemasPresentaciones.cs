using Dominio.Nucleo;
using Dominio.Entidades;
using Presentaciones.Interfaces;

namespace Presentaciones.Implementaciones
{
    public class TemasPresentacion : ITemasPresentacion
    {
        private Comunicaciones? comunicaciones = null;
        private readonly AuditoriasPresentacion auditor = new AuditoriasPresentacion();

        public async Task<List<Temas>> Listar()
        {
            var lista = new List<Temas>();
            var datos = new Dictionary<string, object>();

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Temas/Listar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            lista = JsonConversor.ConvertirAObjeto<List<Temas>>(
                JsonConversor.ConvertirAString(respuesta["Entidades"]));

            //  Registrar en log
            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Temas",
                Accion = "Listar",
                UsuarioAccion = "Sistema",
                DatosAntes = null,
                DatosDespues = $"Se listaron {lista.Count} Temas.",
                FechaAccion = DateTime.Now
            });

            return lista;
        }

        public async Task<List<Temas>> PorTemas(Temas? entidad)
        {
            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad!;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Temas/PorTemas");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            var lista = JsonConversor.ConvertirAObjeto<List<Temas>>(
                JsonConversor.ConvertirAString(respuesta["Entidades"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Temas",
                Accion = "Consulta por usuario",
                UsuarioAccion = "Sistema",
                DatosAntes = JsonConversor.ConvertirAString(entidad),
                DatosDespues = $"Resultado: {lista.Count} registros.",
                FechaAccion = DateTime.Now
            });

            return lista;
        }

        public async Task<Temas?> Guardar(Temas? entidad)
        {
            if (entidad!.Id != 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Temas/Guardar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            entidad = JsonConversor.ConvertirAObjeto<Temas>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Temas",
                EntidadId = entidad.Id,
                Accion = "Guardar",
                UsuarioAccion = "Sistema",
                DatosAntes = null,
                DatosDespues = JsonConversor.ConvertirAString(entidad),
                FechaAccion = DateTime.Now
            });

            return entidad;
        }

        public async Task<Temas?> Modificar(Temas? entidad)
        {
            if (entidad!.Id == 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Temas/Modificar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            var entidadNueva = JsonConversor.ConvertirAObjeto<Temas>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Temas",
                EntidadId = entidadNueva.Id,
                Accion = "Modificar",
                UsuarioAccion = "Sistema",
                DatosAntes = JsonConversor.ConvertirAString(entidad),
                DatosDespues = JsonConversor.ConvertirAString(entidadNueva),
                FechaAccion = DateTime.Now
            });

            return entidadNueva;
        }

        public async Task<Temas?> Borrar(Temas? entidad)
        {
            if (entidad!.Id == 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Temas/Borrar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            var resultado = JsonConversor.ConvertirAObjeto<Temas>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Temas",
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