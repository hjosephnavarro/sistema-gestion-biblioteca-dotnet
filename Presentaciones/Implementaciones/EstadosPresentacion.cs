/*
using Dominio.Nucleo;
using Dominio.Entidades;
using Presentaciones.Interfaces;

namespace Presentaciones.Implementaciones
{
    /// <summary>
    /// Implementación de la capa de presentación para la gestión de estados.
    /// Contiene métodos para listar, filtrar, guardar, modificar y borrar estados,
    /// así como para consultar información de auditorías relacionadas.
    /// </summary>
    public class EstadosPresentacion : IEstadosPresentacion
    {
        private Comunicaciones? comunicaciones = null;

        /// <summary>
        /// Lista todos los estados registrados.
        /// </summary>
        /// <returns>Una lista de entidades <see cref="Estados"/>.</returns>
        public async Task<List<Estados>> Listar()
        {
            var lista = new List<Estados>();
            var datos = new Dictionary<string, object>();
            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Estados/Listar");

            var respuesta = await comunicaciones!.Execute(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            lista = JsonConversor.ConvertirAObjeto<List<Estados>>(
                JsonConversor.ConvertirAString(respuesta["Entidades"]));

            return lista;
        }

        /// <summary>
        /// Obtiene una lista de estados filtrados según los campos especificados en la entidad proporcionada.
        /// </summary>
        /// <param name="entidad">Entidad <see cref="Estados"/> con los criterios de búsqueda.</param>
        /// <returns>Lista filtrada de estados.</returns>
        public async Task<List<Estados>> PorEstados(Estados? entidad)
        {
            var lista = new List<Estados>();
            var datos = new Dictionary<string, object> { ["Entidad"] = entidad! };

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Estados/PorEstados");

            var respuesta = await comunicaciones!.Execute(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            lista = JsonConversor.ConvertirAObjeto<List<Estados>>(
                JsonConversor.ConvertirAString(respuesta["Entidades"]));

            return lista;
        }

        /// <summary>
        /// Guarda un nuevo estado en el sistema.
        /// </summary>
        /// <param name="entidad">Entidad <see cref="Estados"/> que se desea guardar.</param>
        /// <returns>La entidad <see cref="Estados"/> guardada con su nuevo Id.</returns>
        /// <exception cref="Exception">Se lanza si la entidad tiene un Id distinto de cero o falta información.</exception>
        public async Task<Estados?> Guardar(Estados? entidad)
        {
            if (entidad!.Id != 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object> { ["Entidad"] = entidad };
            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Estados/Guardar");

            var respuesta = await comunicaciones!.Execute(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            entidad = JsonConversor.ConvertirAObjeto<Estados>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            return entidad;
        }

        /// <summary>
        /// Modifica los datos de un estado existente.
        /// </summary>
        /// <param name="entidad">Entidad <see cref="Estados"/> con los nuevos valores.</param>
        /// <returns>La entidad modificada.</returns>
        /// <exception cref="Exception">Se lanza si la entidad no contiene un Id válido.</exception>
        public async Task<Estados?> Modificar(Estados? entidad)
        {
            if (entidad!.Id == 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object> { ["Entidad"] = entidad };
            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Estados/Modificar");

            var respuesta = await comunicaciones!.Execute(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            entidad = JsonConversor.ConvertirAObjeto<Estados>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            return entidad;
        }

        /// <summary>
        /// Elimina un estado existente del sistema.
        /// </summary>
        /// <param name="entidad">Entidad <see cref="Estados"/> a eliminar.</param>
        /// <returns>La entidad eliminada (para confirmación).</returns>
        /// <exception cref="Exception">Se lanza si la entidad no tiene un Id válido.</exception>
        public async Task<Estados?> Borrar(Estados? entidad)
        {
            if (entidad!.Id == 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object> { ["Entidad"] = entidad };
            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Estados/Borrar");

            var respuesta = await comunicaciones!.Execute(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            entidad = JsonConversor.ConvertirAObjeto<Estados>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            return entidad;
        }

        // SECCIÓN DE AUDITORÍAS

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

            var respuesta = await comunicaciones!.Execute(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            lista = JsonConversor.ConvertirAObjeto<List<Auditorias>>(
                JsonConversor.ConvertirAString(respuesta["Entidades"]));

            return lista;
        }

        /// <summary>
        /// Obtiene auditorías filtradas por nombre de entidad (por ejemplo, "Estados")
        /// y opcionalmente por Id de entidad.
        /// </summary>
        /// <param name="entidad">Nombre de la entidad (por ejemplo, "Estados").</param>
        /// <param name="entidadId">Id de la entidad (opcional).</param>
        /// <returns>Lista de auditorías filtradas.</returns>
        public async Task<List<Auditorias>> PorEntidadAuditoria(string entidad, int? entidadId)
        {
            var lista = new List<Auditorias>();
            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad;

            if (entidadId.HasValue)
                datos["EntidadId"] = entidadId.Value;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Auditorias/PorEntidad");

            var respuesta = await comunicaciones!.Execute(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            lista = JsonConversor.ConvertirAObjeto<List<Auditorias>>(
                JsonConversor.ConvertirAString(respuesta["Entidades"]));

            return lista;
        }

        /// <summary>
        /// Obtiene las auditorías asociadas a un estado específico.
        /// </summary>
        /// <param name="estado">Entidad <see cref="Estados"/> cuyo Id se usará para filtrar auditorías.</param>
        /// <returns>Lista de auditorías relacionadas con el estado.</returns>
        public async Task<List<Auditorias>> AuditoriasPorEstado(Estados? estado)
        {
            if (estado == null) return new List<Auditorias>();
            return await PorEntidadAuditoria("Estados", estado.Id);
        }
    }
}
*/

using Dominio.Nucleo;
using Dominio.Entidades;
using Presentaciones.Interfaces;

namespace Presentaciones.Implementaciones
{
    public class EstadosPresentacion : IEstadosPresentacion
    {
        private Comunicaciones? comunicaciones = null;
        private readonly AuditoriasPresentacion auditor = new AuditoriasPresentacion();

        public async Task<List<Estados>> Listar()
        {
            var lista = new List<Estados>();
            var datos = new Dictionary<string, object>();

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Estados/Listar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            lista = JsonConversor.ConvertirAObjeto<List<Estados>>(
                JsonConversor.ConvertirAString(respuesta["Entidades"]));

            //  Registrar en log
            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Estados",
                Accion = "Listar",
                UsuarioAccion = "Sistema",
                DatosAntes = null,
                DatosDespues = $"Se listaron {lista.Count} Estados.",
                FechaAccion = DateTime.Now
            });

            return lista;
        }

        public async Task<List<Estados>> PorEstados(Estados? entidad)
        {
            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad!;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Estados/PorEstados");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            var lista = JsonConversor.ConvertirAObjeto<List<Estados>>(
                JsonConversor.ConvertirAString(respuesta["Entidades"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Estados",
                Accion = "Consulta por usuario",
                UsuarioAccion = "Sistema",
                DatosAntes = JsonConversor.ConvertirAString(entidad),
                DatosDespues = $"Resultado: {lista.Count} registros.",
                FechaAccion = DateTime.Now
            });

            return lista;
        }

        public async Task<Estados?> Guardar(Estados? entidad)
        {
            if (entidad!.Id != 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Estados/Guardar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            entidad = JsonConversor.ConvertirAObjeto<Estados>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Estados",
                EntidadId = entidad.Id,
                Accion = "Guardar",
                UsuarioAccion = "Sistema",
                DatosAntes = null,
                DatosDespues = JsonConversor.ConvertirAString(entidad),
                FechaAccion = DateTime.Now
            });

            return entidad;
        }

        public async Task<Estados?> Modificar(Estados? entidad)
        {
            if (entidad!.Id == 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Estados/Modificar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            var entidadNueva = JsonConversor.ConvertirAObjeto<Estados>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Estados",
                EntidadId = entidadNueva.Id,
                Accion = "Modificar",
                UsuarioAccion = "Sistema",
                DatosAntes = JsonConversor.ConvertirAString(entidad),
                DatosDespues = JsonConversor.ConvertirAString(entidadNueva),
                FechaAccion = DateTime.Now
            });

            return entidadNueva;
        }

        public async Task<Estados?> Borrar(Estados? entidad)
        {
            if (entidad!.Id == 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Estados/Borrar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            var resultado = JsonConversor.ConvertirAObjeto<Estados>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Estados",
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