using Dominio.Entidades;
using Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Repositorio.Implementaciones
{
    public partial class Conexion : DbContext, IConexion
    {
        public string? StringConexion { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.StringConexion!, p => { });
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        public DbSet<Autores>? Autores { get; set; }
        public DbSet<Libros>? Libros { get; set; }
        public DbSet<Editoriales>? Editoriales { get; set; }
        public DbSet<Paises>? Paises { get; set; }
        public DbSet<Tipos>? Tipos { get; set; }
        public DbSet<Existencias>? Existencias { get; set; }
        public DbSet<Estados>? Estados { get; set; }
        public DbSet<EstadosExistencias>? EstadosExistencias { get; set; }
        public DbSet<Prestamos>? Prestamos { get; set; }
        public DbSet<Sanciones>? Sanciones { get; set; }
        public DbSet<LibrosAutores>? LibrosAutores { get; set; }
        public DbSet<LibrosTemas>? LibrosTemas { get; set; }
        public DbSet<Temas>? Temas { get; set; }
        public DbSet<TiposPrestamos>? TiposPrestamos { get; set; }
        public DbSet<Usuarios>? Usuarios { get; set; }
        public DbSet<Auditorias>? Auditorias { get; set; }
    }
}