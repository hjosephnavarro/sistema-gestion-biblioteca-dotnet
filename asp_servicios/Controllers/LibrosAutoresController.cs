using asp_servicios.Nucleo;
using Aplicacion.Interfaces;
using Dominio.Entidades;
using Dominio.Nucleo;
using Microsoft.AspNetCore.Mvc;

namespace asp_servicios.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LibrosAutoresController : ControllerBase
    {
        private readonly ILibrosAutoresAplicacion _iAplicacion;
        private readonly TokenController _tokenController;

        public LibrosAutoresController(ILibrosAutoresAplicacion iAplicacion, TokenController tokenController)
        {
            _iAplicacion = iAplicacion;
            _tokenController = tokenController;
        }

        private Dictionary<string, object> ObtenerDatos()
        {
            using var reader = new StreamReader(Request.Body);
            var datos = reader.ReadToEnd();
            if (string.IsNullOrEmpty(datos))
                datos = "{}";

            return JsonConversor.ConvertirAObjeto(datos);
        }

        private string RespuestaOK(Dictionary<string, object> datos)
        {
            datos["Respuesta"] = "OK";
            datos["Fecha"] = DateTime.Now.ToString();
            return JsonConversor.ConvertirAString(datos);
        }

        private string RespuestaError(Exception ex)
        {
            return JsonConversor.ConvertirAString(new Dictionary<string, object> { ["Error"] = ex.Message });
        }

        [HttpPost]
        public string Listar()
        {
            try
            {
                var datos = ObtenerDatos();
                if (!_tokenController.Validate(datos))
                    return JsonConversor.ConvertirAString(new { Error = "lbNoAutenticacion" });

                _iAplicacion.Configurar(Configuracion.ObtenerValor("StringConexion") ?? string.Empty);
                var entidades = _iAplicacion.Listar();

                return RespuestaOK(new Dictionary<string, object> { ["Entidades"] = entidades });
            }
            catch (Exception ex) { return RespuestaError(ex); }
        }

        [HttpPost]
        public string Guardar()
        {
            try
            {
                var datos = ObtenerDatos();
                if (!_tokenController.Validate(datos))
                    return JsonConversor.ConvertirAString(new { Error = "lbNoAutenticacion" });

                var entidadJson = datos.ContainsKey("Entidad") && datos["Entidad"] != null
                    ? JsonConversor.ConvertirAString(datos["Entidad"])
                    : "{}";

                var entidad = JsonConversor.ConvertirAObjeto<LibrosAutores>(entidadJson);
                _iAplicacion.Configurar(Configuracion.ObtenerValor("StringConexion") ?? string.Empty);

                // Validar duplicado (Libro + Autor)
                var existentes = _iAplicacion.Listar();
                if (existentes.Any(x => x.Libro == entidad.Libro && x.Autor == entidad.Autor))
                    throw new Exception("Ya existe una relación entre este libro y autor.");

                var resultado = _iAplicacion.Guardar(entidad);
                return RespuestaOK(new Dictionary<string, object> { ["Entidad"] = resultado });
            }
            catch (Exception ex) { return RespuestaError(ex); }
        }

        [HttpPost]
        public string Modificar()
        {
            try
            {
                var datos = ObtenerDatos();
                if (!_tokenController.Validate(datos))
                    return JsonConversor.ConvertirAString(new { Error = "lbNoAutenticacion" });

                var entidadJson = datos.ContainsKey("Entidad") && datos["Entidad"] != null
                    ? JsonConversor.ConvertirAString(datos["Entidad"])
                    : "{}";

                var entidad = JsonConversor.ConvertirAObjeto<LibrosAutores>(entidadJson);
                _iAplicacion.Configurar(Configuracion.ObtenerValor("StringConexion") ?? string.Empty);

                // Validar que no exista otro registro con la misma combinación Libro + Autor
                var existentes = _iAplicacion.Listar();
                if (existentes.Any(x => x.Id != entidad.Id && x.Libro == entidad.Libro && x.Autor == entidad.Autor))
                    throw new Exception("Ya existe otra relación con este libro y autor.");

                var resultado = _iAplicacion.Modificar(entidad);
                return RespuestaOK(new Dictionary<string, object> { ["Entidad"] = resultado });
            }
            catch (Exception ex) { return RespuestaError(ex); }
        }

        [HttpPost]
        public string Borrar()
        {
            try
            {
                var datos = ObtenerDatos();
                if (!_tokenController.Validate(datos))
                    return JsonConversor.ConvertirAString(new { Error = "lbNoAutenticacion" });

                var entidadJson = datos.ContainsKey("Entidad") && datos["Entidad"] != null
                    ? JsonConversor.ConvertirAString(datos["Entidad"])
                    : "{}";

                var entidad = JsonConversor.ConvertirAObjeto<LibrosAutores>(entidadJson);
                _iAplicacion.Configurar(Configuracion.ObtenerValor("StringConexion") ?? string.Empty);

                var resultado = _iAplicacion.Borrar(entidad);
                return RespuestaOK(new Dictionary<string, object> { ["Entidad"] = resultado });
            }
            catch (Exception ex) { return RespuestaError(ex); }
        }
    }
}
