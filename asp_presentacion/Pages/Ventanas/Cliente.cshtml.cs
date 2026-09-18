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
        private readonly ITiposPrestamosPresentacion _tiposPrestamosPresentacion;
        private readonly ISancionesPresentacion _sancionesPresentacion;

        public ClienteModel(
            IUsuariosPresentacion usuariosPresentacion,
            ILibrosPresentacion librosPresentacion,
            IExistenciasPresentacion existenciasPresentacion,
            IPrestamosPresentacion prestamosPresentacion,
            ITiposPrestamosPresentacion tiposPrestamosPresentacion,
            ISancionesPresentacion sancionesPresentacion)
        {
            _usuariosPresentacion = usuariosPresentacion;
            _librosPresentacion = librosPresentacion;
            _existenciasPresentacion = existenciasPresentacion;
            _prestamosPresentacion = prestamosPresentacion;
            _tiposPrestamosPresentacion = tiposPrestamosPresentacion;
            _sancionesPresentacion = sancionesPresentacion;
        }

        public Usuarios? Usuario { get; set; }

        public class BookViewModel
        {
            public Libros Libro { get; set; } = null!;
            public int Total { get; set; }
            public int Available { get; set; }
        }

        public List<BookViewModel> Books { get; set; } = new();

        public IActionResult OnGet()
        {
            int? userId = HttpContext.Session.GetInt32("UsuarioId");
            if (userId == null || userId == 0)
            {
                return RedirectToPage("/Ventanas/Login");
            }

            Usuario = _usuariosPresentacion.Listar().Result.FirstOrDefault(u => u.Id == userId.Value);
            if (Usuario == null)
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Ventanas/Login");
            }

            HttpContext.Session.SetString("UsuarioNombre", Usuario.Nombre ?? "");

            var libros = _librosPresentacion.Listar().Result ?? new List<Libros>();
            var existencias = _existenciasPresentacion.Listar().Result ?? new List<Existencias>();

            Books = libros.Select(l =>
            {
                var eList = existencias.Where(e => e.Libro == l.Id).ToList();
                var total = eList.Sum(e => e.Ejemplares);
                var available = total;
                return new BookViewModel { Libro = l, Total = total, Available = available };
            })
            .Where(b => b.Total > 0)
            .OrderBy(b => b.Libro.Titulo)
            .ToList();

            return Page();
        }

        public class SolicitarRequest 
        { 
            public int LibroId { get; set; } 
        }

        public async Task<IActionResult> OnPostSolicitarPrestamoAjax([FromBody] SolicitarRequest request)
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("UsuarioId");
                if (userId == null || userId == 0)
                {
                    return new JsonResult(new { 
                        success = false, 
                        message = "Sesion invalida. Vuelve a iniciar sesion." 
                    });
                }

                if (request == null || request.LibroId <= 0)
                {
                    return new JsonResult(new { 
                        success = false, 
                        message = "Peticion invalida." 
                    });
                }

                var existencias = await _existenciasPresentacion.Listar();
                var existencia = existencias
                    .Where(e => e.Libro == request.LibroId && e.Ejemplares > 0)
                    .OrderByDescending(e => e.Ejemplares)
                    .FirstOrDefault();

                if (existencia == null)
                {
                    return new JsonResult(new { 
                        success = false, 
                        message = "No hay ejemplares disponibles para ese libro." 
                    });
                }

                var tiposPrestamos = await _tiposPrestamosPresentacion.Listar();
                var tipoPrestamoId = tiposPrestamos.FirstOrDefault()?.Id ?? 1;

                var prestamo = new Prestamos
                {
                    Usuario = userId.Value,
                    Existencia = existencia.Id,
                    Tipo_Prestamo = tipoPrestamoId,
                    Fecha_Prestamo = DateTime.Now,
                    Fecha_Devolucion = DateTime.Now.AddDays(7),
                    Fecha_Entrega_Real = null
                };

                var prestamoGuardado = await _prestamosPresentacion.Guardar(prestamo);

                if (prestamoGuardado == null || prestamoGuardado.Id == 0)
                {
                    return new JsonResult(new { 
                        success = false, 
                        message = "Error al guardar el prestamo. Intenta nuevamente." 
                    });
                }

                existencia.Ejemplares -= 1;
                var existenciaActualizada = await _existenciasPresentacion.Modificar(existencia);

                if (existenciaActualizada == null)
                {
                    return new JsonResult(new { 
                        success = false, 
                        message = "Error al actualizar existencias." 
                    });
                }

                return new JsonResult(new { 
                    success = true, 
                    message = "Prestamo realizado correctamente.", 
                    available = existencia.Ejemplares,
                    prestamoId = prestamoGuardado.Id
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { 
                    success = false, 
                    message = $"Error: {ex.Message}" 
                });
            }
        }

        public async Task<IActionResult> OnPostPrestamosActivosAjax()
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("UsuarioId");
                if (userId == null || userId == 0)
                {
                    return new JsonResult(new { error = "No autorizado" });
                }

                var todosPrestamos = await _prestamosPresentacion.Listar();
                var todosLibros = await _librosPresentacion.Listar();
                var todasExistencias = await _existenciasPresentacion.Listar();

                var prestamosUsuario = todosPrestamos.Where(p => p.Usuario == userId.Value).ToList();
                var librosDict = todosLibros.ToDictionary(l => l.Id, l => l);
                var existenciasDict = todasExistencias.ToDictionary(e => e.Id, e => e);

                var prestamosActivos = prestamosUsuario
                    .Where(p => p.Fecha_Entrega_Real == null)
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
                            tipoPrestamo = "Estandar"
                        };
                    }).ToList();

                return new JsonResult(new
                {
                    prestamos = prestamosActivos
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }

        // METODO PARA OBTENER SANCIONES DEL USUARIO
        public async Task<IActionResult> OnPostSancionesAjax()
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("UsuarioId");
                if (userId == null || userId == 0)
                {
                    return new JsonResult(new { error = "No autorizado" });
                }

                var todasSanciones = await _sancionesPresentacion.Listar();
                var sancionesUsuario = todasSanciones
                    .Where(s => s.Usuario == userId.Value)
                    .Select(s => new
                    {
                        id = s.Id,
                        descripcion = s.Descripcion,
                        fechaInicio = s.Fecha_Inicio.ToString("yyyy-MM-dd"),
                        fechaFin = s.Fecha_Fin?.ToString("yyyy-MM-dd") ?? "",
                        activa = s.Fecha_Fin == null || s.Fecha_Fin > DateTime.Now
                    })
                    .ToList();

                return new JsonResult(new
                {
                    sanciones = sancionesUsuario
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Ventanas/Login");
        }
    }
}