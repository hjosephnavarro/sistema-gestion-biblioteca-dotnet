using asp_presentacion; 
using asp_presentacion.Middleware;

var builder = WebApplication.CreateBuilder(args); 

var startup = new Startup(builder.Configuration); 
startup.ConfigureServices(builder, builder.Services); 

builder.Services.AddHttpContextAccessor();

var app = builder.Build(); 
startup.Configure(app, app.Environment); 
app.UseMiddleware<SessionMiddleware>();
app.Run(); 