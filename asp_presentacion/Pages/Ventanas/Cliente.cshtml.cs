using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Dominio.Entidades;
using Presentaciones.Interfaces;

namespace asp_presentacion.Pages.Ventanas
{
    public class ClienteModel : PageModel
    {
        private readonly IUsuariosPresentacion _usuariosPresentacion;
        private readonly ILibrosPresentacion _librosPresentacion;
        private readonly IExistenciasPresentacion _existenciasPresentacion;
        private readonly IPrestamosPresentacion _prestamosPresentacion;

        public ClienteModel(
            IUsuariosPresentacion usuariosPresentacion,
            ILibrosPresentacion librosPresentacion,
            IExistenciasPresentacion existenciasPresentacion,
            IPrestamosPresentacion prestamosPresentacion)
        {
            _usuariosPresentacion = usuariosPresentacion;
            _librosPresentacion = librosPresentacion;
            _existenciasPresentacion = existenciasPresentacion;
            _prestamosPresentacion = prestamosPresentacion;
        }

        // Usuario en sesi�n
        public Usuarios? Usuario { get; set; }

        // ViewModel por libro con totales
        public class BookViewModel
        {
            public Libros Libro { get; set; } = null!;
            public int Total { get; set; }
            public int Available { get; set; }
        }

        public List<BookViewModel> Books { get; set; } = new();

        public IActionResult OnGet()
        {
            // comprobar sesi�n
            int? userId = HttpContext.Session.GetInt32("UsuarioId");
            if (userId == null) return RedirectToPage("/Ventanas/Login");

            // obtener usuario (usamos .Result porque las presentaciones en tu proyecto suelen exponer tareas)
            Usuario = _usuariosPresentacion.Listar().Result.FirstOrDefault(u => u.Id == userId.Value);
            if (Usuario == null)
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Ventanas/Login");
            }

            // cargar datos
            var libros = _librosPresentacion.Listar().Result ?? new List<Libros>();
            var existencias = _existenciasPresentacion.Listar().Result ?? new List<Existencias>();

            // mapear a Books con Total/Available (=suma de ejemplares en existencias)
            Books = libros.Select(l =>
            {
                var eList = existencias.Where(e => e.Libro == l.Id).ToList();
                var total = eList.Sum(e => e.Ejemplares);
                var available = total; // si tienes estados, aqu� podr�as restar prestados, etc.
                return new BookViewModel { Libro = l, Total = total, Available = available };
            })
            .Where(b => b.Total > 0) // mostrar solo libros con existencias > 0
            .OrderBy(b => b.Libro.Titulo)
            .ToList();

            return Page();
        }

        // Petici�n AJAX: { libroId: 123 }
        public class SolicitarRequest { public int LibroId { get; set; } }

        [IgnoreAntiforgeryToken] // opcional si manejas token de otra forma; si usas antiforgery, quita esta l�nea
        public IActionResult OnPostSolicitarPrestamoAjax([FromBody] SolicitarRequest request)
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("UsuarioId");
                if (userId == null) return new JsonResult(new { success = false, message = "Sesi�n inv�lida. Vuelve a iniciar sesi�n." });

                if (request == null || request.LibroId <= 0)
                    return new JsonResult(new { success = false, message = "Petici�n inv�lida." });

                // buscar una existencia disponible para ese libro
                var existencia = _existenciasPresentacion.Listar().Result
                    .Where(e => e.Libro == request.LibroId && e.Ejemplares > 0)
                    .OrderByDescending(e => e.Ejemplares)
                    .FirstOrDefault();

                if (existencia == null)
                    return new JsonResult(new { success = false, message = "No hay ejemplares disponibles para ese libro." });

                // crear entidad Prestamos
                var prestamo = new Prestamos
                {
                    Usuario = userId.Value,
                    Existencia = existencia.Id,
                    Tipo_Prestamo = 1, // por defecto (ajusta seg�n tu l�gica)
                    Fecha_Prestamo = DateTime.UtcNow,
                    Fecha_Devolucion = DateTime.UtcNow.AddDays(7)
                };

                // Guardar pr�stamo (presentaci�n delega a aplicaci�n/repositorio)
                _prestamosPresentacion.Guardar(prestamo);

                // decrementar existencias en 1 y guardar
                existencia.Ejemplares -= 1;
                _existenciasPresentacion.Modificar(existencia);

                return new JsonResult(new { success = true, message = "Pr�stamo realizado correctamente.", available = existencia.Ejemplares });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Ventanas/Login");
        }
    }
}