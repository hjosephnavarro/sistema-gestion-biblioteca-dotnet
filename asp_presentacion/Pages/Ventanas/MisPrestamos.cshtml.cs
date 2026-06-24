// MisPrestamos.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Dominio.Entidades;
using Presentaciones.Interfaces;

namespace asp_presentacion.Pages.Ventanas
{
    public class MisPrestamosModel : PageModel
    {
        private readonly IUsuariosPresentacion _usuariosPresentacion;
        private readonly IPrestamosPresentacion _prestamosPresentacion;
        private readonly ILibrosPresentacion _librosPresentacion;
        private readonly ISancionesPresentacion _sancionesPresentacion;

        public MisPrestamosModel(
            IUsuariosPresentacion usuariosPresentacion,
            IPrestamosPresentacion prestamosPresentacion,
            ILibrosPresentacion librosPresentacion,
            ISancionesPresentacion sancionesPresentacion)
        {
            _usuariosPresentacion = usuariosPresentacion;
            _prestamosPresentacion = prestamosPresentacion;
            _librosPresentacion = librosPresentacion;
            _sancionesPresentacion = sancionesPresentacion;
        }

        public string UsuarioNombre { get; set; } = string.Empty;
        public int UsuarioId { get; set; }

        public IActionResult OnGet()
        {
            int? userId = HttpContext.Session.GetInt32("UsuarioId");
            if (userId == null) return RedirectToPage("/Ventanas/Login");
            
            UsuarioId = userId.Value;
            var usuario = _usuariosPresentacion.Listar().Result?.FirstOrDefault(u => u.Id == UsuarioId);
            UsuarioNombre = usuario?.Nombre ?? "Usuario";
            
            return Page();
        }

        public async Task<JsonResult> OnPostDatosPrestamosAjax()
        {
            int? userId = HttpContext.Session.GetInt32("UsuarioId");
            if (userId == null) return new JsonResult(new { error = "No autorizado" });

            var todosPrestamos = (await _prestamosPresentacion.Listar()).Where(p => p.Usuario == userId.Value).ToList();
            var libros = (await _librosPresentacion.Listar()).ToDictionary(l => l.Id, l => l);
            var sanciones = (await _sancionesPresentacion.Listar()).Where(s => s.Usuario == userId.Value).ToList();

            var prestamosActivos = todosPrestamos
                .Where(p => p.Fecha_Entrega_Real == null)
                .Select(p => new
                {
                    id = p.Id,
                    libroTitulo = libros.ContainsKey(p.ExistenciaNavigation?.Libro ?? 0) ? libros[p.ExistenciaNavigation.Libro].Titulo : "Desconocido",
                    isbn = libros.ContainsKey(p.ExistenciaNavigation?.Libro ?? 0) ? libros[p.ExistenciaNavigation.Libro].Isbn : "",
                    fechaPrestamo = p.Fecha_Prestamo.ToString("yyyy-MM-dd"),
                    fechaDevolucion = p.Fecha_Devolucion?.ToString("yyyy-MM-dd") ?? "",
                    renovado = false
                }).ToList();

            var prestamosHistorial = todosPrestamos
                .Where(p => p.Fecha_Entrega_Real != null)
                .Select(p => new
                {
                    id = p.Id,
                    libroTitulo = libros.ContainsKey(p.ExistenciaNavigation?.Libro ?? 0) ? libros[p.ExistenciaNavigation.Libro].Titulo : "Desconocido",
                    isbn = libros.ContainsKey(p.ExistenciaNavigation?.Libro ?? 0) ? libros[p.ExistenciaNavigation.Libro].Isbn : "",
                    fechaPrestamo = p.Fecha_Prestamo.ToString("yyyy-MM-dd"),
                    fechaDevolucion = p.Fecha_Devolucion?.ToString("yyyy-MM-dd") ?? "",
                    fechaEntregaReal = p.Fecha_Entrega_Real?.ToString("yyyy-MM-dd") ?? ""
                }).ToList();

            var sancionesList = sanciones.Select(s => new
            {
                id = s.Id,
                descripcion = s.Descripcion,
                fechaInicio = s.Fecha_Inicio.ToString("yyyy-MM-dd"),
                fechaFin = s.Fecha_Fin?.ToString("yyyy-MM-dd")
            }).ToList();

            return new JsonResult(new
            {
                prestamosActivos,
                prestamosHistorial,
                sanciones = sancionesList
            });
        }

        public async Task<JsonResult> OnPostRenovarPrestamoAjax([FromBody] RenovarRequest request)
        {
            int? userId = HttpContext.Session.GetInt32("UsuarioId");
            if (userId == null) return new JsonResult(new { success = false, message = "No autorizado" });

            var prestamo = (await _prestamosPresentacion.Listar()).FirstOrDefault(p => p.Id == request.PrestamoId && p.Usuario == userId.Value);
            if (prestamo == null) return new JsonResult(new { success = false, message = "Préstamo no encontrado" });
            
            if (prestamo.Fecha_Entrega_Real != null)
                return new JsonResult(new { success = false, message = "Este préstamo ya fue devuelto" });
            
            if (prestamo.Fecha_Devolucion < DateTime.UtcNow.Date)
                return new JsonResult(new { success = false, message = "Préstamo vencido, no se puede renovar" });

            prestamo.Fecha_Devolucion = prestamo.Fecha_Devolucion?.AddDays(7);
            await _prestamosPresentacion.Modificar(prestamo);
            
            return new JsonResult(new { success = true, message = "Préstamo renovado por 7 días más" });
        }

        public class RenovarRequest
        {
            public int PrestamoId { get; set; }
        }
    }
}