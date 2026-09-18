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
        private readonly IExistenciasPresentacion _existenciasPresentacion;

        public MisPrestamosModel(
            IUsuariosPresentacion usuariosPresentacion,
            IPrestamosPresentacion prestamosPresentacion,
            ILibrosPresentacion librosPresentacion,
            ISancionesPresentacion sancionesPresentacion,
            IExistenciasPresentacion existenciasPresentacion)
        {
            _usuariosPresentacion = usuariosPresentacion;
            _prestamosPresentacion = prestamosPresentacion;
            _librosPresentacion = librosPresentacion;
            _sancionesPresentacion = sancionesPresentacion;
            _existenciasPresentacion = existenciasPresentacion;
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

            // Obtener todos los datos
            var todosPrestamos = await _prestamosPresentacion.Listar();
            var todosLibros = await _librosPresentacion.Listar();
            var todasExistencias = await _existenciasPresentacion.Listar();
            var sanciones = await _sancionesPresentacion.Listar();

            // Filtrar por usuario
            var prestamosUsuario = todosPrestamos.Where(p => p.Usuario == userId.Value).ToList();
            
            // Crear diccionarios para búsqueda rápida
            var librosDict = todosLibros.ToDictionary(l => l.Id, l => l);
            var existenciasDict = todasExistencias.ToDictionary(e => e.Id, e => e);

            // Préstamos activos (sin fecha de entrega real)
            var prestamosActivos = prestamosUsuario
                .Where(p => p.Fecha_Entrega_Real == null)
                .Select(p => 
                {
                    // Obtener la existencia y el libro
                    var existencia = existenciasDict.ContainsKey(p.Existencia) ? existenciasDict[p.Existencia] : null;
                    var libro = (existencia != null && librosDict.ContainsKey(existencia.Libro)) ? librosDict[existencia.Libro] : null;
                    
                    return new
                    {
                        id = p.Id,
                        libroTitulo = libro?.Titulo ?? "Libro no encontrado",
                        isbn = libro?.Isbn ?? "N/A",
                        fechaPrestamo = p.Fecha_Prestamo.ToString("yyyy-MM-dd"),
                        fechaDevolucion = p.Fecha_Devolucion?.ToString("yyyy-MM-dd") ?? "",
                        renovado = false
                    };
                }).ToList();

            // Historial (con fecha de entrega real)
            var prestamosHistorial = prestamosUsuario
                .Where(p => p.Fecha_Entrega_Real != null)
                .Select(p =>
                {
                    var existencia = existenciasDict.ContainsKey(p.Existencia) ? existenciasDict[p.Existencia] : null;
                    var libro = (existencia != null && librosDict.ContainsKey(existencia.Libro)) ? librosDict[existencia.Libro] : null;
                    
                    return new
                    {
                        id = p.Id,
                        libroTitulo = libro?.Titulo ?? "Libro no encontrado",
                        isbn = libro?.Isbn ?? "N/A",
                        fechaPrestamo = p.Fecha_Prestamo.ToString("yyyy-MM-dd"),
                        fechaDevolucion = p.Fecha_Devolucion?.ToString("yyyy-MM-dd") ?? "",
                        fechaEntregaReal = p.Fecha_Entrega_Real?.ToString("yyyy-MM-dd") ?? ""
                    };
                }).ToList();

            // Sanciones del usuario
            var sancionesList = sanciones
                .Where(s => s.Usuario == userId.Value)
                .Select(s => new
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