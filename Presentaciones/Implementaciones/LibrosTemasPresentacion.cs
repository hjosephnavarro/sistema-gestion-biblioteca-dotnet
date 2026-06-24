using Dominio.Nucleo;
using Dominio.Entidades;
using Presentaciones.Interfaces;

namespace Presentaciones.Implementaciones
{
    public class LibrosTemasPresentacion : ILibrosTemasPresentacion
    {
        private Comunicaciones? comunicaciones = null;
        private readonly AuditoriasPresentacion auditor = new AuditoriasPresentacion();

        public async Task<List<LibrosTemas>> Listar()
        {
            var lista = new List<LibrosTemas>();
            var datos = new Dictionary<string, object>();

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "LibrosTemas/Listar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            lista = JsonConversor.ConvertirAObjeto<List<LibrosTemas>>(
                JsonConversor.ConvertirAString(respuesta["Entidades"]));

            //  Registrar en log
            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "LibrosTemas",
                Accion = "Listar",
                UsuarioAccion = "Sistema",
                DatosAntes = null,
                DatosDespues = $"Se listaron {lista.Count} LibrosTemas.",
                FechaAccion = DateTime.Now
            });

            return lista;
        }

        public async Task<List<LibrosTemas>> PorLibrosTemas(LibrosTemas? entidad)
        {
            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad!;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "LibrosTemas/PorLibrosTemas");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            var lista = JsonConversor.ConvertirAObjeto<List<LibrosTemas>>(
                JsonConversor.ConvertirAString(respuesta["Entidades"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "LibrosTemas",
                Accion = "Consulta por usuario",
                UsuarioAccion = "Sistema",
                DatosAntes = JsonConversor.ConvertirAString(entidad),
                DatosDespues = $"Resultado: {lista.Count} registros.",
                FechaAccion = DateTime.Now
            });

            return lista;
        }

        public async Task<LibrosTemas?> Guardar(LibrosTemas? entidad)
        {
            if (entidad!.Id != 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "LibrosTemas/Guardar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            entidad = JsonConversor.ConvertirAObjeto<LibrosTemas>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "LibrosTemas",
                EntidadId = entidad.Id,
                Accion = "Guardar",
                UsuarioAccion = "Sistema",
                DatosAntes = null,
                DatosDespues = JsonConversor.ConvertirAString(entidad),
                FechaAccion = DateTime.Now
            });

            return entidad;
        }

        public async Task<LibrosTemas?> Modificar(LibrosTemas? entidad)
        {
            if (entidad!.Id == 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "LibrosTemas/Modificar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            var entidadNueva = JsonConversor.ConvertirAObjeto<LibrosTemas>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "LibrosTemas",
                EntidadId = entidadNueva.Id,
                Accion = "Modificar",
                UsuarioAccion = "Sistema",
                DatosAntes = JsonConversor.ConvertirAString(entidad),
                DatosDespues = JsonConversor.ConvertirAString(entidadNueva),
                FechaAccion = DateTime.Now
            });

            return entidadNueva;
        }

        public async Task<LibrosTemas?> Borrar(LibrosTemas? entidad)
        {
            if (entidad!.Id == 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "LibrosTemas/Borrar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            var resultado = JsonConversor.ConvertirAObjeto<LibrosTemas>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "LibrosTemas",
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
