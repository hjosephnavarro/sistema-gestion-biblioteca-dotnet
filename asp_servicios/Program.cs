using asp_servicios;

// var usuario = "Test.Trghhjsgdj"; // El usuario que deseas encriptar
// var usuarioEncriptado = EncriptarConversor.Encriptar(usuario); // Encriptamos el valor

// Mostrar el valor encriptado en consola (opcional, puedes eliminarlo si no es necesario)
// Console.WriteLine($"Usuario Encriptado: {usuarioEncriptado}");

// Asignamos el valor encriptado a la propiedad estática usuario_datos
// DatosGenerales.usuario_datos = usuarioEncriptado;

// Imprimir para verificar que se asignó correctamente
// Console.WriteLine($"usuario_datos asignado: {DatosGenerales.usuario_datos}");

// Posman de http://localhost:5100/Token/Autenticar
// "Usuario": "SBWq+oLWYfEUF42esqPTIw=="

// Posman de http://localhost:5100/Usuarios/Listar (Entidades)
// { "Bearer": "Cadena del token" }

var builder = WebApplication.CreateBuilder(args);

// Toda la configuración JSON se hará dentro del Startup.
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder, builder.Services);

// Construir la aplicación
var app = builder.Build();

// Configurar la aplicación (middlewares, enrutamiento, etc.)
startup.Configure(app, app.Environment);

// Iniciar la aplicación
app.Run();
