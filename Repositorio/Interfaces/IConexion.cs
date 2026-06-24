using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Repositorio.Interfaces
{
    public interface IConexion
    {
        string? StringConexion { get; set; }

        DbSet<Autores>? Autores { get; set; }
        DbSet<Libros>? Libros { get; set; }
        DbSet<Editoriales>? Editoriales { get; set; }
        DbSet<Paises>? Paises { get; set; }
        DbSet<Tipos>? Tipos { get; set; }
        DbSet<Existencias>? Existencias { get; set; }
        DbSet<Estados>? Estados { get; set; }
        DbSet<EstadosExistencias>? EstadosExistencias { get; set; }
        DbSet<Prestamos>? Prestamos { get; set; }
        DbSet<Sanciones>? Sanciones { get; set; }
        DbSet<LibrosAutores>? LibrosAutores { get; set; }
        DbSet<LibrosTemas>? LibrosTemas { get; set; }
        DbSet<Temas>? Temas { get; set; }
        DbSet<TiposPrestamos>? TiposPrestamos { get; set; }
        DbSet<Usuarios>? Usuarios { get; set; }
        DbSet<Auditorias>? Auditorias { get; set; }

        EntityEntry<T> Entry<T>(T entity) where T : class;
        int SaveChanges();
    }
}