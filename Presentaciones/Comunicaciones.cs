using Dominio.Nucleo;
using System.Text;

namespace Presentaciones
{
    public class Comunicaciones
    {
        private static readonly HttpClient _httpClient = new HttpClient() { Timeout = new TimeSpan(0, 4, 0) };

        private string? URL = string.Empty;
        private string? llave = null;

        public Comunicaciones(string url = "http://localhost:5100")
        {
            URL = url.EndsWith("/") ? url : url + "/";
        }

        // Mantener compatibilidad: Url (principal) y UrlLlave / UrlToken
        public Dictionary<string, object> ConstruirUrl(Dictionary<string, object> data, string Metodo)
        {
            data["Url"] = URL + Metodo;
            data["UrlLlave"] = URL + "Token/Llave";
            data["UrlToken"] = URL + "Token/Autenticar"; // compatibilidad con otros proyectos
            return data;
        }

        // Método existente (lógica)
        public async Task<Dictionary<string, object>> Ejecutar(Dictionary<string, object> datos)
        {
            var respuesta = new Dictionary<string, object>();
            try
            {
                // Obtener llave/token
                var llaveResp = await Llave(datos);
                if (llaveResp == null || llaveResp.ContainsKey("Error"))
                    return llaveResp ?? new Dictionary<string, object> { ["Error"] = "lbErrorLlave" };

                //  Nuevo: pedir token JWT
                var tokenResp = await Token(datos);
                if (tokenResp == null || tokenResp.ContainsKey("Error"))
                    return tokenResp ?? new Dictionary<string, object> { ["Error"] = "lbErrorToken" };
                // final

                // limpiar respuesta temporal
                respuesta.Clear();

                if (!datos.TryGetValue("Url", out var urlObj) || urlObj == null)
                    return new Dictionary<string, object> { ["Error"] = "lbUrlNoProporcionada" };

                var url = urlObj.ToString() ?? string.Empty;

                // preparar payload
                datos.Remove("Url");
                datos.Remove("UrlLlave");
                datos.Remove("UrlToken");
                datos["Llave"] = llave ?? string.Empty;

                var stringData = JsonConversor.ConvertirAString(datos);

                using var content = new StringContent(stringData, Encoding.UTF8, "application/json");
                //var message = await _httpClient.PostAsync(url, content);
                using var message = await _httpClient.PostAsync(url, content);

                //cambiado (if (!message.IsSuccessStatusCode)
                //return new Dictionary<string, object> { ["Error"] = "lbErrorComunicacion", ["Status"] = (int)message.StatusCode };)
                
                if (!message.IsSuccessStatusCode)
                {
                    return new Dictionary<string, object>
                    {
                        ["Error"] = "lbErrorComunicacion",
                        ["Status"] = (int)message.StatusCode,
                        ["Mensaje"] = await message.Content.ReadAsStringAsync()
                    };
                }

                var resp = await message.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(resp))
                    return new Dictionary<string, object> { ["Error"] = "lbRespuestaVacia" };

                resp = Replace(resp);

                //cambiado
                // try
                // {
                //    respuesta = JsonConversor.ConvertirAObjeto(resp);
                //}
                //catch (Exception ex)
                //{
                //    return new Dictionary<string, object> { ["Error"] = "lbErrorParseRespuesta", ["Detalle"] = ex.Message, ["Raw"] = resp };
                //}

                try
                {
                    respuesta = JsonConversor.ConvertirAObjeto(resp);
                }
                catch (Exception ex)
                {
                    return new Dictionary<string, object>
                    {
                        ["Error"] = "lbErrorParseRespuesta",
                        ["Detalle"] = ex.Message,
                        ["Raw"] = resp
                    };
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                return new Dictionary<string, object> { ["Error"] = "lbExcepcionComunicacion", ["Detalle"] = ex.ToString() };
            }
        }

        // ---- ADAPTADOR: Execute -> Ejecutar (compatibilidad)
        public Task<Dictionary<string, object>> Execute(Dictionary<string, object> datos)
        {
            // solo forward a Ejecutar para que el resto del código no falle
            return Ejecutar(datos);
        }

        private async Task<Dictionary<string, object>> Llave(Dictionary<string, object> datos)
        {
            var respuesta = new Dictionary<string, object>();
            try
            {
                if (!datos.TryGetValue("UrlLlave", out var urlObj) || urlObj == null)
                    return new Dictionary<string, object> { ["Error"] = "lbUrlLlaveNoProporcionada" };

                var url = urlObj.ToString() ?? string.Empty;

                var temp = new Dictionary<string, object>
                {
                    ["Entidad"] = new Dictionary<string, object>()
                    {
                        { "Nombre", "Pepito@email.com" },
                        { "Contraseña", "JHGjkhtu6387456yssdf" }
                    }
                };

                var stringData = JsonConversor.ConvertirAString(temp);
                using var content = new StringContent(stringData, Encoding.UTF8, "application/json");
                var mensaje = await _httpClient.PostAsync(url, content);

                if (!mensaje.IsSuccessStatusCode)
                    return new Dictionary<string, object> { ["Error"] = "lbErrorComunicacion", ["Status"] = (int)mensaje.StatusCode };

                var resp = await mensaje.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(resp))
                    return new Dictionary<string, object> { ["Error"] = "lbRespuestaVacia" };

                resp = Replace(resp);
                try
                {
                    respuesta = JsonConversor.ConvertirAObjeto(resp);
                }
                catch (Exception ex)
                {
                    return new Dictionary<string, object> { ["Error"] = "lbErrorParseLlave", ["Detalle"] = ex.Message, ["Raw"] = resp };
                }

                if (respuesta.TryGetValue("Llave", out var llaveObj) && llaveObj != null)
                    llave = llaveObj.ToString();

                return respuesta;
            }
            catch (Exception ex)
            {
                return new Dictionary<string, object> { ["Error"] = "lbExcepcionLlave", ["Detalle"] = ex.ToString() };
            }
        }

        // inicio
            private async Task<Dictionary<string, object>> Token(Dictionary<string, object> datos)
        {
            var respuesta = new Dictionary<string, object>();
            try
            {
                if (!datos.TryGetValue("UrlToken", out var urlObj) || urlObj == null)
                    return new Dictionary<string, object> { ["Error"] = "lbUrlTokenNoProporcionada" };

                var url = urlObj.ToString() ?? string.Empty;

                var payload = new Dictionary<string, object>()
        {
            { "Usuario", DatosGenerales.usuario_datos }  // debe coincidir con backend
        };

                var stringData = JsonConversor.ConvertirAString(payload);
                using var content = new StringContent(stringData, Encoding.UTF8, "application/json");

                var mensaje = await _httpClient.PostAsync(url, content);

                if (!mensaje.IsSuccessStatusCode)
                    return new Dictionary<string, object> { ["Error"] = "lbErrorComunicacionToken", ["Status"] = (int)mensaje.StatusCode };

                var resp = await mensaje.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(resp))
                    return new Dictionary<string, object> { ["Error"] = "lbRespuestaVaciaToken" };

                resp = Replace(resp);
                respuesta = JsonConversor.ConvertirAObjeto(resp);

                if (respuesta.TryGetValue("Token", out var tokenObj) && tokenObj != null)
                    datos["Bearer"] = "Bearer " + tokenObj.ToString();

                return respuesta;
            }
            catch (Exception ex)
            {
                return new Dictionary<string, object> { ["Error"] = "lbExcepcionToken", ["Detalle"] = ex.ToString() };
            }
        }
        // final

        /*
         * {
         * "Entidad": {
         * "Nombre": "Pepito@email.com",
         * "Contraseña": "JHGjkhtu6387456yssdf"
         * }
         * }
         */

        private string Replace(string resp)
        {
            if (resp == null) return string.Empty;
            // la "mutilación"
            return resp.Replace("\\\\r\\\\n", "")
                       .Replace("\\r\\n", "")
                       .Replace("\\", "")
                       .Replace("\\\"", "\"")
                       .Replace("\"", "'")
                       .Replace("'[", "[")
                       .Replace("]'", "]")
                       .Replace("'{", "{'")
                       .Replace("\\\\", "\\")
                       .Replace("'}'", "'}")
                       .Replace("}'", "}")
                       .Replace("\\n", "")
                       .Replace("\\r", "")
                       .Replace(" ", "")
                       .Replace("'{", "{")
                       .Replace("\"", "")
                       .Replace(" ", "")
                       .Replace("null", "''");
        }
    }
}

//Si llave es vacia error, en la comunicacion


/*
using Dominio.Entidades;
using Dominio.Nucleo;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Presentaciones
{
    internal class Comunicaciones
    {
        private static readonly HttpClient _httpClient = new HttpClient() { Timeout = new TimeSpan(0, 4, 0) };
        private string? Protocolo = string.Empty;
        private string? Host = string.Empty;
        private string? Servicio = string.Empty;
        private string? token = null;

        public Comunicaciones(string servicio = "asp_servicios/", string protocolo = "http://", string host = "localhost")
        {
            Protocolo = protocolo;
            Host = host;
            Servicio = servicio;
        }

        public Dictionary<string, object> ConstruirUrl(Dictionary<string, object> data, string Metodo)
        {
            data["Url"] = $"{Protocolo}{Host}/{Servicio}{Metodo}";
            data["UrlToken"] = $"{Protocolo}{Host}/{Servicio}Token/Autenticar";
            return data;
        }

        public async Task<Dictionary<string, object>> Execute(Dictionary<string, object> datos)
        {
            var respuesta = new Dictionary<string, object>();

            try
            {
                // Authenticate: se espera que Authenticate devuelva { "Token": "..."} o { "Error": "..." }
                var auth = await Authenticate(datos);
                if (auth == null || auth.ContainsKey("Error"))
                    return auth ?? new Dictionary<string, object> { ["Error"] = "lbErrorAutenticacion" };

                // Obtener URL segura (TryGetValue)
                if (!datos.TryGetValue("Url", out var urlObj) || urlObj == null)
                    return new Dictionary<string, object> { ["Error"] = "lbUrlNoProporcionada" };

                var url = urlObj.ToString() ?? string.Empty;

                // Si Authenticate devolvió token, úsalo
                if (!auth.TryGetValue("Token", out var tokenObj) || tokenObj == null)
                    return new Dictionary<string, object> { ["Error"] = "lbTokenNoObtenido" };

                token = tokenObj.ToString();

                // Preparamos payload: eliminamos Url/UrlToken antes de serializar
                datos.Remove("Url");
                datos.Remove("UrlToken");
                datos["Bearer"] = token ?? string.Empty;

                var stringData = JsonConversor.ConvertirAString(datos);

                using var content = new StringContent(stringData, Encoding.UTF8, "application/json");
                var message = await _httpClient.PostAsync(url, content);

                if (!message.IsSuccessStatusCode)
                {
                    return new Dictionary<string, object> { ["Error"] = "lbErrorComunicacion" };
                }

                var resp = await message.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(resp))
                    return new Dictionary<string, object> { ["Error"] = "lbRespuestaVacia" };

                // Intentar convertir directamente; no "mutilar" el JSON.
                try
                {
                    respuesta = JsonConversor.ConvertirAObjeto(resp);
                }
                catch (Exception ex)
                {
                    // Si falla el parse, devolvemos el error para debug
                    return new Dictionary<string, object> { ["Error"] = "lbErrorParseRespuesta", ["Detalle"] = ex.Message, ["Raw"] = resp };
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                return new Dictionary<string, object> { ["Error"] = "lbExcepcionComunicacion", ["Detalle"] = ex.ToString() };
            }
        }

        private async Task<Dictionary<string, object>> Authenticate(Dictionary<string, object> datos)
        {
            var respuesta = new Dictionary<string, object>();

            try
            {
                if (!datos.TryGetValue("UrlToken", out var urlTokenObj) || urlTokenObj == null)
                    return new Dictionary<string, object> { ["Error"] = "lbUrlTokenNoProporcionada" };

                var url = urlTokenObj.ToString() ?? string.Empty;

                var temp = new Dictionary<string, object>
                {
                    ["Usuario"] = DatosGenerales.usuario_datos ?? string.Empty
                };

                var stringData = JsonConversor.ConvertirAString(temp);
                using var content = new StringContent(stringData, Encoding.UTF8, "application/json");
                var mensaje = await _httpClient.PostAsync(url, content);

                if (!mensaje.IsSuccessStatusCode)
                    return new Dictionary<string, object> { ["Error"] = "lbErrorComunicacion" };

                var resp = await mensaje.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(resp))
                    return new Dictionary<string, object> { ["Error"] = "lbRespuestaVacia" };

                try
                {
                    respuesta = JsonConversor.ConvertirAObjeto(resp);
                }
                catch (Exception ex)
                {
                    return new Dictionary<string, object> { ["Error"] = "lbErrorParseAutenticacion", ["Detalle"] = ex.Message, ["Raw"] = resp };
                }

                // Validar que contenga Token
                if (!respuesta.ContainsKey("Token"))
                    return new Dictionary<string, object> { ["Error"] = "lbTokenNoEnRespuesta", ["Raw"] = respuesta };

                return respuesta;
            }
            catch (Exception ex)
            {
                return new Dictionary<string, object> { ["Error"] = "lbExcepcionAutenticacion", ["Detalle"] = ex.ToString() };
            }
        }

        // Eliminé la mutilación agresiva. Si necesitas normalizar algo, hacerlo con parsers JSON.
        private string Replace(string resp)
        {
            // método mantenido por compatibilidad si algún legacy lo necesita,
            // pero ahora lo dejamos prácticamente neutro:
            return resp?.Trim() ?? string.Empty;
        }
    }
}
*/