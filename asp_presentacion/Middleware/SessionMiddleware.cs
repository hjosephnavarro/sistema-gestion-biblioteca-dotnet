using Microsoft.AspNetCore.Http;
using Presentaciones.Interfaces;

namespace asp_presentacion.Middleware
{
    public class SessionMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUsuariosPresentacion usuariosPresentacion)
        {
            var userId = context.Session.GetInt32("UsuarioId");
            var userName = context.Session.GetString("UsuarioNombre");

            // Si hay usuario en Session pero no nombre, cargarlo
            if (userId.HasValue && userId > 0 && string.IsNullOrEmpty(userName))
            {
                try
                {
                    var usuarios = await usuariosPresentacion.Listar();
                    var user = usuarios.FirstOrDefault(u => u.Id == userId.Value);
                    if (user != null)
                    {
                        context.Session.SetString("UsuarioNombre", user.Nombre ?? "");
                        context.Session.SetString("UsuarioRol", user.Rol ?? "");
                    }
                }
                catch
                {
                    // Si hay error, limpiar sesión
                    context.Session.Clear();
                }
            }

            await _next(context);
        }
    }
}