/*
using Dominio.Nucleo;
using Dominio.Entidades;
using Presentaciones.Interfaces;

namespace Presentaciones.Implementaciones
{
    /// <summary>
    /// Implementación de la capa de presentación para la gestión de editoriales.
    /// Contiene métodos para listar, filtrar, guardar, modificar y borrar editoriales,
    /// así como para consultar información de auditorías relacionadas.
    /// </summary>
    public class EditorialesPresentacion : IEditorialesPresentacion
    {
        private Comunicaciones? comunicaciones = null;

        /// <summary>
        /// Lista todas las editoriales registradas.
        /// </summary>
        /// <returns>Una lista de entidades <see cref="Editoriales"/>.</returns>
        public async Task<List<Editoriales>> Listar()
        {
            var lista = new List<Editoriales>();
            var datos = new Dictionary<string, object>();
            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Editoriales/Listar");
            var respuesta = await comunicaciones!.Execute(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            lista = JsonConversor.ConvertirAObjeto<List<Editoriales>>(
                JsonConversor.ConvertirAString(respuesta["Entidades"]));

            return lista;
        }

        /// <summary>
        /// Obtiene una lista de editoriales filtradas según los campos especificados en la entidad proporcionada.
        /// </summary>
        /// <param name="entidad">Entidad <see cref="Editoriales"/> con los criterios de búsqueda.</param>
        /// <returns>Lista filtrada de editoriales.</returns>
        public async Task<List<Editoriales>> PorEditoriales(Editoriales? entidad)
        {
            var lista = new List<Editoriales>();
            var datos = new Dictionary<string, object> { ["Entidad"] = entidad! };

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Editoriales/PorEditoriales");

            var respuesta = await comunicaciones!.Execute(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            lista = JsonConversor.ConvertirAObjeto<List<Editoriales>>(
                JsonConversor.ConvertirAString(respuesta["Entidades"]));

            return lista;
        }

        /// <summary>
        /// Guarda una nueva editorial en el sistema.
        /// </summary>
        /// <param name="entidad">Entidad <see cref="Editoriales"/> que se desea guardar.</param>
        /// <returns>La entidad <see cref="Editoriales"/> guardada con su nuevo Id.</returns>
        /// <exception cref="Exception">Se lanza si la entidad tiene un Id distinto de cero o falta información.</exception>
        public async Task<Editoriales?> Guardar(Editoriales? entidad)
        {
            if (entidad!.Id != 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object> { ["Entidad"] = entidad };
            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Editoriales/Guardar");

            var respuesta = await comunicaciones!.Execute(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            entidad = JsonConversor.ConvertirAObjeto<Editoriales>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            return entidad;
        }

        /// <summary>
        /// Modifica los datos de una editorial existente.
        /// </summary>
        /// <param name="entidad">Entidad <see cref="Editoriales"/> con los nuevos valores.</param>
        /// <returns>La entidad modificada.</returns>
        /// <exception cref="Exception">Se lanza si la entidad no contiene un Id válido.</exception>
        public async Task<Editoriales?> Modificar(Editoriales? entidad)
        {
            if (entidad!.Id == 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object> { ["Entidad"] = entidad };
            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Editoriales/Modificar");

            var respuesta = await comunicaciones!.Execute(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            entidad = JsonConversor.ConvertirAObjeto<Editoriales>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            return entidad;
        }

        /// <summary>
        /// Elimina una editorial existente del sistema.
        /// </summary>
        /// <param name="entidad">Entidad <see cref="Editoriales"/> a eliminar.</param>
        /// <returns>La entidad eliminada (para confirmación).</returns>
        /// <exception cref="Exception">Se lanza si la entidad no tiene un Id válido.</exception>
        public async Task<Editoriales?> Borrar(Editoriales? entidad)
        {
            if (entidad!.Id == 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object> { ["Entidad"] = entidad };
            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Editoriales/Borrar");

            var respuesta = await comunicaciones!.Execute(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            entidad = JsonConversor.ConvertirAObjeto<Editoriales>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

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

            var respuesta = await comunicaciones!.Execute(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            lista = JsonConversor.ConvertirAObjeto<List<Auditorias>>(
                JsonConversor.ConvertirAString(respuesta["Entidades"]));

            return lista;
        }

        /// <summary>
        /// Obtiene auditorías filtradas por nombre de entidad (por ejemplo, "Editoriales")
        /// y opcionalmente por Id de entidad.
        /// </summary>
        /// <param name="entidad">Nombre de la entidad (por ejemplo, "Editoriales").</param>
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
        /// Obtiene las auditorías asociadas a una editorial específica.
        /// </summary>
        /// <param name="editorial">Entidad <see cref="Editoriales"/> cuyo Id se usará para filtrar auditorías.</param>
        /// <returns>Lista de auditorías relacionadas con la editorial.</returns>
        public async Task<List<Auditorias>> AuditoriasPorEditorial(Editoriales? editorial)
        {
            if (editorial == null) return new List<Auditorias>();
            return await PorEntidadAuditoria("Editoriales", editorial.Id);
        }
    }
}
*/

using Dominio.Nucleo;
using Dominio.Entidades;
using Presentaciones.Interfaces;

namespace Presentaciones.Implementaciones
{
    public class EditorialesPresentacion : IEditorialesPresentacion
    {
        private Comunicaciones? comunicaciones = null;
        private readonly AuditoriasPresentacion auditor = new AuditoriasPresentacion();

        public async Task<List<Editoriales>> Listar()
        {
            var lista = new List<Editoriales>();
            var datos = new Dictionary<string, object>();

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Editoriales/Listar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            lista = JsonConversor.ConvertirAObjeto<List<Editoriales>>(
                JsonConversor.ConvertirAString(respuesta["Entidades"]));

            //  Registrar en log
            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Editoriales",
                Accion = "Listar",
                UsuarioAccion = "Sistema",
                DatosAntes = null,
                DatosDespues = $"Se listaron {lista.Count} Editoriales.",
                FechaAccion = DateTime.Now
            });

            return lista;
        }

        public async Task<List<Editoriales>> PorEditoriales(Editoriales? entidad)
        {
            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad!;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Editoriales/PorEditoriales");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            var lista = JsonConversor.ConvertirAObjeto<List<Editoriales>>(
                JsonConversor.ConvertirAString(respuesta["Entidades"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Editoriales",
                Accion = "Consulta por usuario",
                UsuarioAccion = "Sistema",
                DatosAntes = JsonConversor.ConvertirAString(entidad),
                DatosDespues = $"Resultado: {lista.Count} registros.",
                FechaAccion = DateTime.Now
            });

            return lista;
        }

        public async Task<Editoriales?> Guardar(Editoriales? entidad)
        {
            if (entidad!.Id != 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Editoriales/Guardar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            entidad = JsonConversor.ConvertirAObjeto<Editoriales>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Editoriales",
                EntidadId = entidad.Id,
                Accion = "Guardar",
                UsuarioAccion = "Sistema",
                DatosAntes = null,
                DatosDespues = JsonConversor.ConvertirAString(entidad),
                FechaAccion = DateTime.Now
            });

            return entidad;
        }

        public async Task<Editoriales?> Modificar(Editoriales? entidad)
        {
            if (entidad!.Id == 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Editoriales/Modificar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            var entidadNueva = JsonConversor.ConvertirAObjeto<Editoriales>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Editoriales",
                EntidadId = entidadNueva.Id,
                Accion = "Modificar",
                UsuarioAccion = "Sistema",
                DatosAntes = JsonConversor.ConvertirAString(entidad),
                DatosDespues = JsonConversor.ConvertirAString(entidadNueva),
                FechaAccion = DateTime.Now
            });

            return entidadNueva;
        }

        public async Task<Editoriales?> Borrar(Editoriales? entidad)
        {
            if (entidad!.Id == 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Editoriales/Borrar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            var resultado = JsonConversor.ConvertirAObjeto<Editoriales>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Editoriales",
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
