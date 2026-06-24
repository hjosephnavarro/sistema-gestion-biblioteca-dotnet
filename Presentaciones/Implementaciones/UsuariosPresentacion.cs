/*
using Dominio.Nucleo;
using Dominio.Entidades;
using Presentaciones.Interfaces;

namespace Presentaciones.Implementaciones
{
    /// <summary>
    /// Implementación de la capa de presentación para la gestión de usuarios.
    /// Contiene métodos para listar, filtrar, guardar, modificar y borrar usuarios,
    /// así como para consultar auditorías relacionadas.
    /// </summary>
    public class UsuariosPresentacion : IUsuariosPresentacion
    {
        private Comunicaciones? comunicaciones = null;

        /// <summary>
        /// Lista todos los usuarios registrados.
        /// </summary>
        /// <returns>Una lista de entidades <see cref="Usuarios"/>.</returns>
        public async Task<List<Usuarios>> Listar()
        {
            var lista = new List<Usuarios>();
            var datos = new Dictionary<string, object>();

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Usuarios/Listar");

            var respuesta = await comunicaciones.Execute(datos)
                ?? throw new Exception("lbErrorComunicacion"); // por seguridad

            if (respuesta.TryGetValue("Error", out var err))
                throw new Exception(err?.ToString() ?? "lbErrorDesconocido");

            if (!respuesta.TryGetValue("Entidades", out var entidadesObj) || entidadesObj == null)
                throw new Exception("lbRespuestaInvalida");

            lista = JsonConversor.ConvertirAObjeto<List<Usuarios>>(
                JsonConversor.ConvertirAString(entidadesObj));

            return lista;
        }

        /// <summary>
        /// Obtiene una lista de usuarios filtrados según los campos especificados en la entidad proporcionada.
        /// </summary>
        /// <param name="entidad">Entidad <see cref="Usuarios"/> con los criterios de búsqueda.</param>
        /// <returns>Lista filtrada de usuarios.</returns>
        public async Task<List<Usuarios>> PorUsuario(Usuarios? entidad)
        {
            var lista = new List<Usuarios>();
            var datos = new Dictionary<string, object>();

            if (entidad != null)
                datos["Entidad"] = entidad;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Usuarios/PorUsuario");

            var respuesta = await comunicaciones.Execute(datos)
                ?? throw new Exception("lbErrorComunicacion");

            if (respuesta.TryGetValue("Error", out var err))
                throw new Exception(err?.ToString() ?? "lbErrorDesconocido");

            if (!respuesta.TryGetValue("Entidades", out var entidadesObj) || entidadesObj == null)
                return new List<Usuarios>();

            lista = JsonConversor.ConvertirAObjeto<List<Usuarios>>(
                JsonConversor.ConvertirAString(entidadesObj));

            return lista;
        }

        /// <summary>
        /// Guarda un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="entidad">Entidad <see cref="Usuarios"/> que se desea guardar.</param>
        /// <returns>La entidad <see cref="Usuarios"/> guardada con su nuevo Id.</returns>
        /// <exception cref="Exception">Se lanza si la entidad tiene un Id distinto de cero o falta información.</exception>
        public async Task<Usuarios?> Guardar(Usuarios? entidad)
        {
            if (entidad == null || entidad.Id != 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object> { ["Entidad"] = entidad };
            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Usuarios/Guardar");

            var respuesta = await comunicaciones.Execute(datos)
                ?? throw new Exception("lbErrorComunicacion");

            if (respuesta.TryGetValue("Error", out var err))
                throw new Exception(err?.ToString() ?? "lbErrorDesconocido");

            if (!respuesta.TryGetValue("Entidad", out var entidadObj) || entidadObj == null)
                throw new Exception("lbRespuestaInvalida");

            entidad = JsonConversor.ConvertirAObjeto<Usuarios>(
                JsonConversor.ConvertirAString(entidadObj));

            return entidad;
        }

        /// <summary>
        /// Modifica los datos de un usuario existente.
        /// </summary>
        /// <param name="entidad">Entidad <see cref="Usuarios"/> con los nuevos valores.</param>
        /// <returns>La entidad modificada.</returns>
        /// <exception cref="Exception">Se lanza si la entidad no contiene un Id válido.</exception>
        public async Task<Usuarios?> Modificar(Usuarios? entidad)
        {
            if (entidad == null || entidad.Id == 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object> { ["Entidad"] = entidad };
            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Usuarios/Modificar");

            var respuesta = await comunicaciones.Execute(datos)
                ?? throw new Exception("lbErrorComunicacion");

            if (respuesta.TryGetValue("Error", out var err))
                throw new Exception(err?.ToString() ?? "lbErrorDesconocido");

            if (!respuesta.TryGetValue("Entidad", out var entidadObj) || entidadObj == null)
                throw new Exception("lbRespuestaInvalida");

            entidad = JsonConversor.ConvertirAObjeto<Usuarios>(
                JsonConversor.ConvertirAString(entidadObj));

            return entidad;
        }

        /// <summary>
        /// Elimina un usuario existente del sistema.
        /// </summary>
        /// <param name="entidad">Entidad <see cref="Usuarios"/> a eliminar.</param>
        /// <returns>La entidad eliminada (para confirmación).</returns>
        /// <exception cref="Exception">Se lanza si la entidad no tiene un Id válido.</exception>
        public async Task<Usuarios?> Borrar(Usuarios? entidad)
        {
            if (entidad == null || entidad.Id == 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object> { ["Entidad"] = entidad };
            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Usuarios/Borrar");

            var respuesta = await comunicaciones.Execute(datos)
                ?? throw new Exception("lbErrorComunicacion");

            if (respuesta.TryGetValue("Error", out var err))
                throw new Exception(err?.ToString() ?? "lbErrorDesconocido");

            if (!respuesta.TryGetValue("Entidad", out var entidadObj) || entidadObj == null)
                throw new Exception("lbRespuestaInvalida");

            entidad = JsonConversor.ConvertirAObjeto<Usuarios>(
                JsonConversor.ConvertirAString(entidadObj));

            return entidad;
        }

        // AUDITORÍA

        /// <summary>
        /// Lista todas las auditorías registradas en el sistema.
        /// </summary>
        /// <returns>Lista de entidades <see cref="Auditorias"/>.</returns>
        public async Task<List<Auditorias>> ListarAuditorias()
        {
            var lista = new List<Auditorias>();
            var datos = new Dictionary<string, object>();
            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Auditorias/Listar");

            var respuesta = await comunicaciones.Execute(datos)
                ?? throw new Exception("lbErrorComunicacion");

            if (respuesta.TryGetValue("Error", out var err))
                throw new Exception(err?.ToString() ?? "lbErrorDesconocido");

            if (!respuesta.TryGetValue("Entidades", out var entidadesObj) || entidadesObj == null)
                return new List<Auditorias>();

            lista = JsonConversor.ConvertirAObjeto<List<Auditorias>>(
                JsonConversor.ConvertirAString(entidadesObj));

            return lista;
        }

        /// <summary>
        /// Obtiene auditorías filtradas por nombre de entidad (por ejemplo, "Usuarios")
        /// y opcionalmente por Id de entidad.
        /// </summary>
        /// <param name="entidad">Nombre de la entidad (por ejemplo, "Usuarios").</param>
        /// <param name="entidadId">Id de la entidad (opcional).</param>
        /// <returns>Lista de auditorías filtradas.</returns>
        public async Task<List<Auditorias>> PorEntidadAuditoria(string entidad, int? entidadId)
        {
            var lista = new List<Auditorias>();
            var datos = new Dictionary<string, object>
            {
                ["Entidad"] = entidad
            };

            if (entidadId.HasValue)
                datos["EntidadId"] = entidadId.Value;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Auditorias/PorEntidad");

            var respuesta = await comunicaciones.Execute(datos)
                ?? throw new Exception("lbErrorComunicacion");

            if (respuesta.TryGetValue("Error", out var err))
                throw new Exception(err?.ToString() ?? "lbErrorDesconocido");

            if (!respuesta.TryGetValue("Entidades", out var entidadesObj) || entidadesObj == null)
                return new List<Auditorias>();

            lista = JsonConversor.ConvertirAObjeto<List<Auditorias>>(
                JsonConversor.ConvertirAString(entidadesObj));

            return lista;
        }

        /// <summary>
        /// Obtiene las auditorías asociadas a un usuario específico.
        /// </summary>
        /// <param name="usuario">Entidad <see cref="Usuarios"/> cuyo Id se usará para filtrar auditorías.</param>
        /// <returns>Lista de auditorías relacionadas con el usuario.</returns>
        public async Task<List<Auditorias>> AuditoriasPorUsuario(Usuarios? usuario)
        {
            if (usuario == null) return new List<Auditorias>();
            return await PorEntidadAuditoria("Usuarios", usuario.Id);
        }
    }
}
*/

using Dominio.Nucleo;
using Dominio.Entidades;
using Presentaciones.Interfaces;

namespace Presentaciones.Implementaciones
{
    public class UsuariosPresentacion : IUsuariosPresentacion
    {
        private Comunicaciones? comunicaciones = null;
        private readonly AuditoriasPresentacion auditor = new AuditoriasPresentacion();

        public async Task<List<Usuarios>> Listar()
        {
            var lista = new List<Usuarios>();
            var datos = new Dictionary<string, object>();

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Usuarios/Listar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            lista = JsonConversor.ConvertirAObjeto<List<Usuarios>>(
                JsonConversor.ConvertirAString(respuesta["Entidades"]));

            //  Registrar en log
            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Usuarios",
                Accion = "Listar",
                UsuarioAccion = "Sistema",
                DatosAntes = null,
                DatosDespues = $"Se listaron {lista.Count} usuarios.",
                FechaAccion = DateTime.Now
            });

            return lista;
        }

        public async Task<List<Usuarios>> PorUsuario(Usuarios? entidad)
        {
            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad!;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Usuarios/PorUsuario");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            var lista = JsonConversor.ConvertirAObjeto<List<Usuarios>>(
                JsonConversor.ConvertirAString(respuesta["Entidades"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Usuarios",
                Accion = "Consulta por usuario",
                UsuarioAccion = "Sistema",
                DatosAntes = JsonConversor.ConvertirAString(entidad),
                DatosDespues = $"Resultado: {lista.Count} registros.",
                FechaAccion = DateTime.Now
            });

            return lista;
        }

        public async Task<Usuarios?> Guardar(Usuarios? entidad)
        {
            if (entidad!.Id != 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Usuarios/Guardar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            entidad = JsonConversor.ConvertirAObjeto<Usuarios>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Usuarios",
                EntidadId = entidad.Id,
                Accion = "Guardar",
                UsuarioAccion = "Sistema",
                DatosAntes = null,
                DatosDespues = JsonConversor.ConvertirAString(entidad),
                FechaAccion = DateTime.Now
            });

            return entidad;
        }

        public async Task<Usuarios?> Modificar(Usuarios? entidad)
        {
            if (entidad!.Id == 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Usuarios/Modificar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            var entidadNueva = JsonConversor.ConvertirAObjeto<Usuarios>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Usuarios",
                EntidadId = entidadNueva.Id,
                Accion = "Modificar",
                UsuarioAccion = "Sistema",
                DatosAntes = JsonConversor.ConvertirAString(entidad),
                DatosDespues = JsonConversor.ConvertirAString(entidadNueva),
                FechaAccion = DateTime.Now
            });

            return entidadNueva;
        }

        public async Task<Usuarios?> Borrar(Usuarios? entidad)
        {
            if (entidad!.Id == 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Usuarios/Borrar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            var resultado = JsonConversor.ConvertirAObjeto<Usuarios>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Usuarios",
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
