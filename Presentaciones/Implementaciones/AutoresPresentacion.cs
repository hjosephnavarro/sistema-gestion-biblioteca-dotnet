/*
using Dominio.Nucleo;
using Dominio.Entidades;
using Presentaciones.Interfaces;

namespace Presentaciones.Implementaciones
{
    /// <summary>
    /// Clase de presentación para gestionar las operaciones de los autores
    /// y su auditoría mediante comunicación con la capa de API.
    /// </summary>
    public class AutoresPresentacion : IAutoresPresentacion
    {
        private Comunicaciones? comunicaciones = null;

        /// <summary>
        /// Lista todos los autores registrados en el sistema.
        /// </summary>
        /// <returns>Una lista con todos los autores.</returns>
        /// <exception cref="Exception">Si ocurre un error en la comunicación o respuesta.</exception>
        public async Task<List<Autores>> Listar()
        {
            var lista = new List<Autores>();
            var datos = new Dictionary<string, object>();
            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Autores/Listar");

            var respuesta = await comunicaciones!.Execute(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            lista = JsonConversor.ConvertirAObjeto<List<Autores>>(
                JsonConversor.ConvertirAString(respuesta["Entidades"]));

            return lista;
        }

        /// <summary>
        /// Obtiene autores filtrados según los campos de la entidad proporcionada.
        /// </summary>
        /// <param name="entidad">Entidad de tipo Autores con los valores de filtro.</param>
        /// <returns>Lista de autores que cumplen con el filtro.</returns>
        /// <exception cref="Exception">Si ocurre un error en la comunicación o respuesta.</exception>
        public async Task<List<Autores>> PorAutores(Autores? entidad)
        {
            var lista = new List<Autores>();
            var datos = new Dictionary<string, object>
            {
                ["Entidad"] = entidad!
            };

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Autores/PorAutores");

            var respuesta = await comunicaciones!.Execute(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            lista = JsonConversor.ConvertirAObjeto<List<Autores>>(
                JsonConversor.ConvertirAString(respuesta["Entidades"]));

            return lista;
        }

        /// <summary>
        /// Guarda un nuevo autor en la base de datos.
        /// </summary>
        /// <param name="entidad">Entidad del autor a guardar.</param>
        /// <returns>El autor guardado con su nuevo Id asignado.</returns>
        /// <exception cref="Exception">Si falta información o hay error en la comunicación.</exception>
        public async Task<Autores?> Guardar(Autores? entidad)
        {
            if (entidad == null || entidad.Id != 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object> { ["Entidad"] = entidad };
            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Autores/Guardar");

            var respuesta = await comunicaciones!.Execute(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            entidad = JsonConversor.ConvertirAObjeto<Autores>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            return entidad;
        }

        /// <summary>
        /// Modifica un autor existente en la base de datos.
        /// </summary>
        /// <param name="entidad">Entidad del autor con los cambios a actualizar.</param>
        /// <returns>El autor modificado.</returns>
        /// <exception cref="Exception">Si falta información o hay error en la comunicación.</exception>
        public async Task<Autores?> Modificar(Autores? entidad)
        {
            if (entidad == null || entidad.Id == 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object> { ["Entidad"] = entidad };
            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Autores/Modificar");

            var respuesta = await comunicaciones!.Execute(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            entidad = JsonConversor.ConvertirAObjeto<Autores>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            return entidad;
        }

        /// <summary>
        /// Elimina un autor existente del sistema.
        /// </summary>
        /// <param name="entidad">Entidad del autor a eliminar.</param>
        /// <returns>El autor eliminado.</returns>
        /// <exception cref="Exception">Si falta información o hay error en la comunicación.</exception>
        public async Task<Autores?> Borrar(Autores? entidad)
        {
            if (entidad == null || entidad.Id == 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object> { ["Entidad"] = entidad };
            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Autores/Borrar");

            var respuesta = await comunicaciones!.Execute(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            entidad = JsonConversor.ConvertirAObjeto<Autores>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            return entidad;
        }

        // AUDITORÍA

        /// <summary>
        /// Lista todas las auditorías registradas en el sistema.
        /// </summary>
        /// <returns>Una lista de auditorías.</returns>
        /// <exception cref="Exception">Si ocurre un error en la comunicación.</exception>
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
        /// Obtiene auditorías filtradas por el nombre de la entidad ("Autores") y opcionalmente por su Id.
        /// </summary>
        /// <param name="entidad">Nombre de la entidad ("Autores").</param>
        /// <param name="entidadId">Id de la entidad, si se desea filtrar un autor específico.</param>
        /// <returns>Lista de auditorías filtradas.</returns>
        /// <exception cref="Exception">Si ocurre un error en la comunicación.</exception>
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
        /// Obtiene todas las auditorías asociadas a un autor específico.
        /// </summary>
        /// <param name="autor">Entidad del autor cuyo historial de auditoría se desea consultar.</param>
        /// <returns>Lista de auditorías del autor indicado.</returns>
        public async Task<List<Auditorias>> AuditoriasPorAutor(Autores? autor)
        {
            if (autor == null) return new List<Auditorias>();
            return await PorEntidadAuditoria("Autores", autor.Id);
        }
    }
}
*/

using Dominio.Nucleo;
using Dominio.Entidades;
using Presentaciones.Interfaces;

namespace Presentaciones.Implementaciones
{
    public class AutoresPresentacion : IAutoresPresentacion
    {
        private Comunicaciones? comunicaciones = null;
        private readonly AuditoriasPresentacion auditor = new AuditoriasPresentacion();

        public async Task<List<Autores>> Listar()
        {
            var lista = new List<Autores>();
            var datos = new Dictionary<string, object>();

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Autores/Listar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            lista = JsonConversor.ConvertirAObjeto<List<Autores>>(
                JsonConversor.ConvertirAString(respuesta["Entidades"]));

            //  Registrar en log
            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Autores",
                Accion = "Listar",
                UsuarioAccion = "Sistema",
                DatosAntes = null,
                DatosDespues = $"Se listaron {lista.Count} Autores.",
                FechaAccion = DateTime.Now
            });

            return lista;
        }

        public async Task<List<Autores>> PorAutores(Autores? entidad)
        {
            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad!;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Autores/PorUsuario");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            var lista = JsonConversor.ConvertirAObjeto<List<Autores>>(
                JsonConversor.ConvertirAString(respuesta["Entidades"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Autores",
                Accion = "Consulta por usuario",
                UsuarioAccion = "Sistema",
                DatosAntes = JsonConversor.ConvertirAString(entidad),
                DatosDespues = $"Resultado: {lista.Count} registros.",
                FechaAccion = DateTime.Now
            });

            return lista;
        }

        public async Task<Autores?> Guardar(Autores? entidad)
        {
            if (entidad!.Id != 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Autores/Guardar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            entidad = JsonConversor.ConvertirAObjeto<Autores>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Autores",
                EntidadId = entidad.Id,
                Accion = "Guardar",
                UsuarioAccion = "Sistema",
                DatosAntes = null,
                DatosDespues = JsonConversor.ConvertirAString(entidad),
                FechaAccion = DateTime.Now
            });

            return entidad;
        }

        public async Task<Autores?> Modificar(Autores? entidad)
        {
            if (entidad!.Id == 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Autores/Modificar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            var entidadNueva = JsonConversor.ConvertirAObjeto<Autores>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Autores",
                EntidadId = entidadNueva.Id,
                Accion = "Modificar",
                UsuarioAccion = "Sistema",
                DatosAntes = JsonConversor.ConvertirAString(entidad),
                DatosDespues = JsonConversor.ConvertirAString(entidadNueva),
                FechaAccion = DateTime.Now
            });

            return entidadNueva;
        }

        public async Task<Autores?> Borrar(Autores? entidad)
        {
            if (entidad!.Id == 0)
                throw new Exception("lbFaltaInformacion");

            var datos = new Dictionary<string, object>();
            datos["Entidad"] = entidad;

            comunicaciones = new Comunicaciones();
            datos = comunicaciones.ConstruirUrl(datos, "Autores/Borrar");
            var respuesta = await comunicaciones!.Ejecutar(datos);

            if (respuesta.ContainsKey("Error"))
                throw new Exception(respuesta["Error"].ToString()!);

            var resultado = JsonConversor.ConvertirAObjeto<Autores>(
                JsonConversor.ConvertirAString(respuesta["Entidad"]));

            await auditor.RegistrarAsync(new Auditorias
            {
                Entidad = "Autores",
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
