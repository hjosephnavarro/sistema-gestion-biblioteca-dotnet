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
        public static IConfiguration? Configuration { get; set; }

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

            // Servicios base
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddRazorPages();
            
            //  CONFIGURACIÓN DE SESIÓN
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //  AGREGAR AUTHENTICATION (necesario para User.Identity)
            services.AddAuthentication();
            services.AddAuthorization();
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // ORDEN CORRECTO DE MIDDLEWARE
            app.UseAuthentication();  // Primero autenticación
            app.UseAuthorization();   // Luego autorización
            app.UseSession();         // Sesión después de autenticación

            app.MapRazorPages();
            app.MapControllers();
            

        }
    }
}