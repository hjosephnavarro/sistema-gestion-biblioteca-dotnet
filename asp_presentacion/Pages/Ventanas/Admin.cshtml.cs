// Admin.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Dominio.Entidades;
using Presentaciones.Interfaces;

namespace asp_presentacion.Pages.Ventanas
{
    public class AdminModel : PageModel
    {
        private readonly IUsuariosPresentacion _usuariosPresentacion;
        private readonly ILibrosPresentacion _librosPresentacion;
        private readonly IAutoresPresentacion _autoresPresentacion;
        private readonly IEditorialesPresentacion _editorialesPresentacion;
        private readonly ITemasPresentacion _temasPresentacion;
        private readonly IExistenciasPresentacion _existenciasPresentacion;
        private readonly IPrestamosPresentacion _prestamosPresentacion;
        private readonly ISancionesPresentacion _sancionesPresentacion;
        private readonly IPaisesPresentacion _paisesPresentacion;
        private readonly ITiposPresentacion _tiposPresentacion;
        private readonly ITiposPrestamosPresentacion _tiposPrestamosPresentacion;
        private readonly ILibrosAutoresPresentacion _librosAutoresPresentacion;
        private readonly ILibrosTemasPresentacion _librosTemasPresentacion;

        public AdminModel(
            IUsuariosPresentacion usuariosPresentacion,
            ILibrosPresentacion librosPresentacion,
            IAutoresPresentacion autoresPresentacion,
            IEditorialesPresentacion editorialesPresentacion,
            ITemasPresentacion temasPresentacion,
            IExistenciasPresentacion existenciasPresentacion,
            IPrestamosPresentacion prestamosPresentacion,
            ISancionesPresentacion sancionesPresentacion,
            IPaisesPresentacion paisesPresentacion,
            ITiposPresentacion tiposPresentacion,
            ITiposPrestamosPresentacion tiposPrestamosPresentacion,
            ILibrosAutoresPresentacion librosAutoresPresentacion,
            ILibrosTemasPresentacion librosTemasPresentacion)
        {
            _usuariosPresentacion = usuariosPresentacion;
            _librosPresentacion = librosPresentacion;
            _autoresPresentacion = autoresPresentacion;
            _editorialesPresentacion = editorialesPresentacion;
            _temasPresentacion = temasPresentacion;
            _existenciasPresentacion = existenciasPresentacion;
            _prestamosPresentacion = prestamosPresentacion;
            _sancionesPresentacion = sancionesPresentacion;
            _paisesPresentacion = paisesPresentacion;
            _tiposPresentacion = tiposPresentacion;
            _tiposPrestamosPresentacion = tiposPrestamosPresentacion;
            _librosAutoresPresentacion = librosAutoresPresentacion;
            _librosTemasPresentacion = librosTemasPresentacion;
        }

        public List<Autores> Autores { get; set; } = new();
        public List<Editoriales> Editoriales { get; set; } = new();
        public List<Libros> Libros { get; set; } = new();
        public List<Temas> Temas { get; set; } = new();
        public List<Existencias> Existencias { get; set; } = new();
        public List<Prestamos> Prestamos { get; set; } = new();
        public List<Sanciones> Sanciones { get; set; } = new();
        public List<Paises> Paises { get; set; } = new();
        public List<Tipos> TiposLibro { get; set; } = new();
        public List<TiposPrestamos> TiposPrestamos { get; set; } = new();
        public List<Usuarios> Usuarios { get; set; } = new();
        public List<LibrosAutores> LibrosAutores { get; set; } = new();
        public List<LibrosTemas> LibrosTemas { get; set; } = new();

        [BindProperty] public Autores NewAutor { get; set; } = new();
        [BindProperty] public Editoriales NewEditorial { get; set; } = new();
        [BindProperty] public Libros NewLibro { get; set; } = new();
        [BindProperty] public Temas NewTema { get; set; } = new();
        [BindProperty] public Existencias NewExistencia { get; set; } = new();
        [BindProperty] public Prestamos NewPrestamo { get; set; } = new();
        [BindProperty] public Sanciones NewSancion { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            int? userId = HttpContext.Session.GetInt32("UsuarioId");
            if (userId == null) return RedirectToPage("/Ventanas/Login");
            
            try
            {
                var usuarios = await _usuariosPresentacion.Listar();
                var usuario = usuarios?.FirstOrDefault(u => u.Id == userId);
                if (usuario?.Rol != "admin") return RedirectToPage("/Ventanas/Cliente");
            }
            catch
            {
                return RedirectToPage("/Ventanas/Login");
            }

            await CargarDatos();
            return Page();
        }

        // Admin.cshtml.cs - Reemplazar el método CargarDatos()
        private async Task CargarDatos()
        {
            var autores = await _autoresPresentacion.Listar();
            Autores = autores?.ToList() ?? new();
            
            var editoriales = await _editorialesPresentacion.Listar();
            Editoriales = editoriales?.ToList() ?? new();
            
            var libros = await _librosPresentacion.Listar();
            Libros = libros?.ToList() ?? new();
            
            var temas = await _temasPresentacion.Listar();
            Temas = temas?.ToList() ?? new();
            
            var existencias = await _existenciasPresentacion.Listar();
            Existencias = existencias?.ToList() ?? new();
            
            var prestamos = await _prestamosPresentacion.Listar();
            Prestamos = prestamos?.ToList() ?? new();
            
            // Manejar error de Sanciones
            try
            {
                var sanciones = await _sancionesPresentacion.Listar();
                Sanciones = sanciones?.ToList() ?? new();
            }
            catch (Exception ex)
            {
                Sanciones = new List<Sanciones>();
                Console.WriteLine($"Error al cargar sanciones: {ex.Message}");
            }
            
            var paises = await _paisesPresentacion.Listar();
            Paises = paises?.ToList() ?? new();
            
            var tipos = await _tiposPresentacion.Listar();
            TiposLibro = tipos?.ToList() ?? new();
            
            var tiposPrestamos = await _tiposPrestamosPresentacion.Listar();
            TiposPrestamos = tiposPrestamos?.ToList() ?? new();
            
            var usuarios = await _usuariosPresentacion.Listar();
            Usuarios = usuarios?.Where(u => u.Rol == "usuario").ToList() ?? new();
            
            var librosAutores = await _librosAutoresPresentacion.Listar();
            LibrosAutores = librosAutores?.ToList() ?? new();
            
            var librosTemas = await _librosTemasPresentacion.Listar();
            LibrosTemas = librosTemas?.ToList() ?? new();
        }

        // LIBROS
        public async Task<IActionResult> OnPostAddLibroAsync()
        {
            if (!string.IsNullOrEmpty(NewLibro.Titulo))
            {
                try
                {
                    await _librosPresentacion.Guardar(NewLibro);
                    TempData["OK"] = "Libro agregado correctamente";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error: " + ex.Message;
                }
            }
            await CargarDatos();
            return Page();
        }

        public async Task<IActionResult> OnPostEditLibroAsync(int id, string titulo, string isbn, string edicion, string fechaLanzamiento)
        {
            var libro = Libros.FirstOrDefault(l => l.Id == id);
            if (libro != null)
            {
                libro.Titulo = titulo;
                libro.Isbn = isbn;
                libro.Edicion = edicion;
                if (!string.IsNullOrEmpty(fechaLanzamiento))
                    libro.Fecha_Lanzamiento = DateTime.Parse(fechaLanzamiento);
                await _librosPresentacion.Modificar(libro);
                TempData["OK"] = "Libro actualizado";
            }
            await CargarDatos();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteLibroAsync(int id)
        {
            var libro = Libros.FirstOrDefault(l => l.Id == id);
            if (libro != null)
            {
                await _librosPresentacion.Borrar(libro);
                TempData["OK"] = "Libro eliminado";
            }
            await CargarDatos();
            return Page();
        }

        public async Task<IActionResult> OnPostAsignarAutorLibroAsync(int libroId, int autorId)
        {
            var existe = LibrosAutores.Any(la => la.Libro == libroId && la.Autor == autorId);
            if (!existe)
            {
                var nuevo = new LibrosAutores { Libro = libroId, Autor = autorId };
                await _librosAutoresPresentacion.Guardar(nuevo);
                TempData["OK"] = "Autor asignado al libro";
            }
            else
            {
                TempData["Error"] = "Este autor ya está asignado a este libro";
            }
            await CargarDatos();
            return Page();
        }

        public async Task<IActionResult> OnPostAsignarTemaLibroAsync(int libroId, int temaId)
        {
            var existe = LibrosTemas.Any(lt => lt.Libro == libroId && lt.Tema == temaId);
            if (!existe)
            {
                var nuevo = new LibrosTemas { Libro = libroId, Tema = temaId };
                await _librosTemasPresentacion.Guardar(nuevo);
                TempData["OK"] = "Tema asignado al libro";
            }
            else
            {
                TempData["Error"] = "Este tema ya está asignado a este libro";
            }
            await CargarDatos();
            return Page();
        }

        // AUTORES
        public async Task<IActionResult> OnPostAddAutorAsync()
        {
            if (!string.IsNullOrEmpty(NewAutor.Nombre))
            {
                await _autoresPresentacion.Guardar(NewAutor);
                TempData["OK"] = "Autor agregado";
            }
            await CargarDatos();
            return Page();
        }

        public async Task<IActionResult> OnPostEditAutorAsync(int id, string nombre, string nacionalidad)
        {
            var autor = Autores.FirstOrDefault(a => a.Id == id);
            if (autor != null)
            {
                autor.Nombre = nombre;
                autor.Nacionalidad = nacionalidad;
                await _autoresPresentacion.Modificar(autor);
                TempData["OK"] = "Autor actualizado";
            }
            await CargarDatos();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAutorAsync(int id)
        {
            var autor = Autores.FirstOrDefault(a => a.Id == id);
            if (autor != null)
            {
                await _autoresPresentacion.Borrar(autor);
                TempData["OK"] = "Autor eliminado";
            }
            await CargarDatos();
            return Page();
        }

        // EDITORIALES
        public async Task<IActionResult> OnPostAddEditorialAsync()
        {
            if (!string.IsNullOrEmpty(NewEditorial.Nombre_Editorial))
            {
                await _editorialesPresentacion.Guardar(NewEditorial);
                TempData["OK"] = "Editorial agregada";
            }
            await CargarDatos();
            return Page();
        }

        public async Task<IActionResult> OnPostEditEditorialAsync(int id, string nombre, string sitioWeb)
        {
            var editorial = Editoriales.FirstOrDefault(e => e.Id == id);
            if (editorial != null)
            {
                editorial.Nombre_Editorial = nombre;
                editorial.Sitio_Web = sitioWeb;
                await _editorialesPresentacion.Modificar(editorial);
                TempData["OK"] = "Editorial actualizada";
            }
            await CargarDatos();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteEditorialAsync(int id)
        {
            var editorial = Editoriales.FirstOrDefault(e => e.Id == id);
            if (editorial != null)
            {
                await _editorialesPresentacion.Borrar(editorial);
                TempData["OK"] = "Editorial eliminada";
            }
            await CargarDatos();
            return Page();
        }

        // TEMAS
        public async Task<IActionResult> OnPostAddTemaAsync()
        {
            if (!string.IsNullOrEmpty(NewTema.Nombre_Tema))
            {
                await _temasPresentacion.Guardar(NewTema);
                TempData["OK"] = "Tema agregado";
            }
            await CargarDatos();
            return Page();
        }

        public async Task<IActionResult> OnPostEditTemaAsync(int id, string nombre, string area)
        {
            var tema = Temas.FirstOrDefault(t => t.Id == id);
            if (tema != null)
            {
                tema.Nombre_Tema = nombre;
                tema.Area_Conocimiento = area;
                await _temasPresentacion.Modificar(tema);
                TempData["OK"] = "Tema actualizado";
            }
            await CargarDatos();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteTemaAsync(int id)
        {
            var tema = Temas.FirstOrDefault(t => t.Id == id);
            if (tema != null)
            {
                await _temasPresentacion.Borrar(tema);
                TempData["OK"] = "Tema eliminado";
            }
            await CargarDatos();
            return Page();
        }

        // EXISTENCIAS
        public async Task<IActionResult> OnPostAddExistenciaAsync()
        {
            if (NewExistencia.Libro > 0 && NewExistencia.Ejemplares > 0)
            {
                var existe = Existencias.FirstOrDefault(e => e.Libro == NewExistencia.Libro);
                if (existe != null)
                {
                    existe.Ejemplares += NewExistencia.Ejemplares;
                    await _existenciasPresentacion.Modificar(existe);
                    TempData["OK"] = "Ejemplares añadidos a existencia existente";
                }
                else
                {
                    await _existenciasPresentacion.Guardar(NewExistencia);
                    TempData["OK"] = "Existencia creada";
                }
            }
            await CargarDatos();
            return Page();
        }

        public async Task<IActionResult> OnPostEditExistenciaAsync(int id, int ejemplares)
        {
            var existencia = Existencias.FirstOrDefault(e => e.Id == id);
            if (existencia != null)
            {
                existencia.Ejemplares = ejemplares;
                await _existenciasPresentacion.Modificar(existencia);
                TempData["OK"] = "Existencia actualizada";
            }
            await CargarDatos();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteExistenciaAsync(int id)
        {
            var existencia = Existencias.FirstOrDefault(e => e.Id == id);
            if (existencia != null)
            {
                await _existenciasPresentacion.Borrar(existencia);
                TempData["OK"] = "Existencia eliminada";
            }
            await CargarDatos();
            return Page();
        }

        // PRESTAMOS
        public async Task<IActionResult> OnPostAddPrestamoAsync()
        {
            if (NewPrestamo.Usuario > 0 && NewPrestamo.Existencia > 0)
            {
                var existencia = Existencias.FirstOrDefault(e => e.Id == NewPrestamo.Existencia);
                if (existencia != null && existencia.Ejemplares > 0)
                {
                    NewPrestamo.Fecha_Prestamo = DateTime.Now;
                    NewPrestamo.Fecha_Devolucion = DateTime.Now.AddDays(7);
                    await _prestamosPresentacion.Guardar(NewPrestamo);
                    
                    existencia.Ejemplares -= 1;
                    await _existenciasPresentacion.Modificar(existencia);
                    TempData["OK"] = "Préstamo registrado";
                }
                else
                {
                    TempData["Error"] = "No hay ejemplares disponibles";
                }
            }
            await CargarDatos();
            return Page();
        }

        public async Task<IActionResult> OnPostRegistrarDevolucionAsync(int id)
        {
            var prestamo = Prestamos.FirstOrDefault(p => p.Id == id);
            if (prestamo != null && prestamo.Fecha_Entrega_Real == null)
            {
                prestamo.Fecha_Entrega_Real = DateTime.Now;
                await _prestamosPresentacion.Modificar(prestamo);
                
                var existencia = Existencias.FirstOrDefault(e => e.Id == prestamo.Existencia);
                if (existencia != null)
                {
                    existencia.Ejemplares += 1;
                    await _existenciasPresentacion.Modificar(existencia);
                }
                TempData["OK"] = "Devolución registrada";
            }
            await CargarDatos();
            return Page();
        }

        public async Task<IActionResult> OnPostDeletePrestamoAsync(int id)
        {
            var prestamo = Prestamos.FirstOrDefault(p => p.Id == id);
            if (prestamo != null)
            {
                await _prestamosPresentacion.Borrar(prestamo);
                TempData["OK"] = "Préstamo eliminado";
            }
            await CargarDatos();
            return Page();
        }

        // SANCIONES
        public async Task<IActionResult> OnPostAddSancionAsync()
        {
            if (NewSancion.Usuario > 0 && !string.IsNullOrEmpty(NewSancion.Descripcion))
            {
                if (NewSancion.Fecha_Inicio == default)
                    NewSancion.Fecha_Inicio = DateTime.Now;
                await _sancionesPresentacion.Guardar(NewSancion);
                TempData["OK"] = "Sanción aplicada";
            }
            await CargarDatos();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteSancionAsync(int id)
        {
            var sancion = Sanciones.FirstOrDefault(s => s.Id == id);
            if (sancion != null)
            {
                await _sancionesPresentacion.Borrar(sancion);
                TempData["OK"] = "Sanción eliminada";
            }
            await CargarDatos();
            return Page();
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Ventanas/Login");
        }
    }
}