using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentaciones.Interfaces;
using Dominio.Entidades;
using Microsoft.AspNetCore.Http;

namespace asp_presentacion.Pages.Ventanas
{
    public class LoginModel : PageModel
    {
        private readonly IUsuariosPresentacion _usuariosPresentacion;

        public LoginModel(IUsuariosPresentacion usuariosPresentacion)
        {
            _usuariosPresentacion = usuariosPresentacion;
        }

        // ===== Bind properties =====
        [BindProperty] public string? LoginCorreo { get; set; }
        [BindProperty] public string? LoginPassword { get; set; }
        [BindProperty] public string LoginRol { get; set; } = "";

        // Registro
        [BindProperty] public string? NuevoNombre { get; set; }
        [BindProperty] public string? NuevoDocumento { get; set; }
        [BindProperty] public string? NuevoDireccion { get; set; }
        [BindProperty] public string? NuevoTelefono { get; set; }
        [BindProperty] public string? NuevoCorreo { get; set; }
        [BindProperty] public string? NuevoPassword { get; set; }
        [BindProperty] public string? NuevoRol { get; set; }
        [BindProperty] public string? ClaveAdmin { get; set; }
        [BindProperty] public DateTime? NuevoFechaNacimiento { get; set; }

        // Estado
        public Usuarios? UsuarioLogueado { get; set; }

        // MÉTODO INTERNO CORRECTO
        private async Task CargarSesion()
        {
            var uid = HttpContext.Session.GetInt32("UsuarioId");

            if (uid.HasValue)
            {
                var lista = await _usuariosPresentacion.Listar();
                UsuarioLogueado = lista.FirstOrDefault(u => u.Id == uid.Value);

                if (UsuarioLogueado != null)
                {
                    var rolSesion = HttpContext.Session.GetString("UsuarioRol");
                    if (string.IsNullOrWhiteSpace(rolSesion))
                        HttpContext.Session.SetString("UsuarioRol", UsuarioLogueado.Rol ?? "");
                }
            }
        }

        // GET
        public async Task OnGet()
        {
            await CargarSesion();
        }

        // LOGIN
        public async Task<IActionResult> OnPostLogin()
        {
            if (string.IsNullOrWhiteSpace(LoginCorreo) ||
                string.IsNullOrWhiteSpace(LoginPassword))
            {
                ViewData["Mensaje"] = "Correo y contraseña requeridos.";
                await CargarSesion();
                return Page();
            }

            var usuarios = await _usuariosPresentacion.Listar();

            var user = usuarios.FirstOrDefault(u =>
                u.Correo.Equals(LoginCorreo, StringComparison.OrdinalIgnoreCase) &&
                u.Contraseña == LoginPassword &&
                (u.Rol ?? "").Equals(LoginRol ?? "", StringComparison.OrdinalIgnoreCase)
            );

            if (user == null)
            {
                ViewData["Mensaje"] = "Credenciales o rol incorrecto.";
                await CargarSesion();
                return Page();
            }

            HttpContext.Session.SetInt32("UsuarioId", user.Id);
            HttpContext.Session.SetString("UsuarioRol", user.Rol ?? "");

            return user.Rol == "admin"
                ? RedirectToPage("/Ventanas/Admin")
                : RedirectToPage("/Ventanas/Cliente");
        }

        // LOGOUT
        public async Task<IActionResult> OnPostLogout()
        {
            HttpContext.Session.Clear();
            ViewData["Mensaje"] = "Sesión cerrada.";
            await CargarSesion();
            return Page();
        }

        // REGISTRO
        public async Task<IActionResult> OnPostCrearUsuario()
        {
            if (string.IsNullOrWhiteSpace(NuevoNombre) ||
                string.IsNullOrWhiteSpace(NuevoDocumento) ||
                string.IsNullOrWhiteSpace(NuevoCorreo) ||
                string.IsNullOrWhiteSpace(NuevoPassword) ||
                string.IsNullOrWhiteSpace(NuevoRol))
            {
                ViewData["Mensaje"] = "Todos los campos son obligatorios.";
                await CargarSesion();
                return Page();
            }

            if (NuevoRol == "admin" && ClaveAdmin != "HjNj2207")
            {
                ViewData["Mensaje"] = "Clave secreta incorrecta.";
                await CargarSesion();
                return Page();
            }

            try
            {
                var nuevo = new Usuarios
                {
                    Nombre = NuevoNombre,
                    Documento = NuevoDocumento,
                    Direccion = NuevoDireccion ?? "",
                    Telefono = NuevoTelefono ?? "",
                    Correo = NuevoCorreo,
                    Contraseña = NuevoPassword,
                    Rol = NuevoRol,
                    Fecha_Nacimiento = NuevoFechaNacimiento
                };

                await _usuariosPresentacion.Guardar(nuevo);
                ViewData["Mensaje"] = "Usuario creado correctamente. Ahora puedes iniciar sesión.";
                
                // Limpiar formulario y mostrar login
                NuevoNombre = null;
                NuevoDocumento = null;
                NuevoDireccion = null;
                NuevoTelefono = null;
                NuevoCorreo = null;
                NuevoPassword = null;
                NuevoRol = null;
                ClaveAdmin = null;
                NuevoFechaNacimiento = null;
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = $"Error: {ex.Message}";
            }

            await CargarSesion();
            return Page();
        }
    }
}