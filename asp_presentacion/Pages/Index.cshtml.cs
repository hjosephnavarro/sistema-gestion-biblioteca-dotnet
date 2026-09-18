using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentaciones.Interfaces;
using Dominio.Entidades;

namespace asp_presentacion.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IUsuariosPresentacion _usuariosPresentacion;
        private readonly ILibrosPresentacion _librosPresentacion;
        private readonly IAutoresPresentacion _autoresPresentacion;

        public List<Libros> ListaLibros { get; set; } = new();
        public List<Autores> ListaAutores { get; set; } = new();
        public List<Usuarios> ListaUsuarios { get; set; } = new();

        public IndexModel(
            IUsuariosPresentacion usuariosPresentacion,
            ILibrosPresentacion librosPresentacion,
            IAutoresPresentacion autoresPresentacion)
        {
            _usuariosPresentacion = usuariosPresentacion;
            _librosPresentacion = librosPresentacion;
            _autoresPresentacion = autoresPresentacion;
        }

        public async Task OnGetAsync()
        {
            try
            {
                ListaLibros = await _librosPresentacion.Listar() ?? new List<Libros>();
            }
            catch (Exception)
            {
                ListaLibros = new List<Libros>();
            }

            try
            {
                ListaAutores = await _autoresPresentacion.Listar() ?? new List<Autores>();
            }
            catch
            {
                ListaAutores = new List<Autores>();
            }

            try
            {
                ListaUsuarios = await _usuariosPresentacion.Listar() ?? new List<Usuarios>();
            }
            catch
            {
                ListaUsuarios = new List<Usuarios>();
            }
        }
    }
}