using Presentaciones.Implementaciones;
using Presentaciones.Interfaces;

namespace asp_presentacion
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public static IConfiguration? Configuration { set; get; }

        public void ConfigureServices(WebApplicationBuilder builder, IServiceCollection services)
        {
            // Presentaciones
            services.AddScoped<IUsuariosPresentacion, UsuariosPresentacion>();
            services.AddScoped<IAutoresPresentacion, AutoresPresentacion>();
            services.AddScoped<IEditorialesPresentacion, EditorialesPresentacion>();
            services.AddScoped<ILibrosPresentacion, LibrosPresentacion>();
            services.AddScoped<ILibrosAutoresPresentacion, LibrosAutoresPresentacion>();
            services.AddScoped<ILibrosTemasPresentacion, LibrosTemasPresentacion>();
            services.AddScoped<IEstadosExistenciasPresentacion, EstadosExistenciasPresentacion>();
            services.AddScoped<IEstadosPresentacion, EstadosPresentacion>();
            services.AddScoped<IExistenciasPresentacion, ExistenciasPresentacion>();
            services.AddScoped<IPaisesPresentacion, PaisesPresentacion>();
            services.AddScoped<IPrestamosPresentacion, PrestamosPresentacion>();
            services.AddScoped<ISancionesPresentacion, SancionesPresentacion>();
            services.AddScoped<ITemasPresentacion, TemasPresentacion>();
            services.AddScoped<ITiposPrestamosPresentacion, TiposPrestamosPresentacion>();
            services.AddScoped<ITiposPresentacion, TiposPresentacion>();
            services.AddScoped<IAuditoriasPresentacion, AuditoriasPresentacion>();

            //services.AddScoped<IUsuariosRolesPresentacion, UsuariosRolesPresentacion>();
            //services.AddScoped<IRolesPresentacion, RolesPresentacion>();
            //services.AddScoped<IUsuariosPermisosPresentacion, UsuariosPermisosPresentacion>();

            // Servicios base
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddRazorPages();
            
            // Configuración de sesión
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseSession();
            app.MapRazorPages();
            app.Run();
        }
    }
}