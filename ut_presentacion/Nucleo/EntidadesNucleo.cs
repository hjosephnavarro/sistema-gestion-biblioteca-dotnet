using Dominio.Entidades;
using System;

namespace ut_presentacion.Nucleo
{
    public class EntidadesNucleo
    {
        public static Autores Autores()
        {
            var entidad = new Autores();
            entidad.Nombre = "Autor Prueba";
            entidad.Nacionalidad = "Desconocida";
            return entidad;
        }

        public static Editoriales Editoriales()
        {
            var entidad = new Editoriales();
            entidad.Nombre_Editorial = "Editorial Prueba";
            entidad.Sitio_Web = "www.prueba.com";
            return entidad;
        }

        public static Paises Paises()
        {
            var entidad = new Paises();
            entidad.Nombre_Pais = "País Prueba";
            entidad.Region = "Región Prueba";
            return entidad;
        }

        public static Tipos Tipos()
        {
            var entidad = new Tipos();
            entidad.Nombre_Tipo = "Tipo Prueba";
            return entidad;
        }

        public static Libros Libros(Editoriales editoriales, Paises paises, Tipos tipos)
        {
            var entidad = new Libros();
            entidad.Titulo = "Libro Prueba";
            entidad.Edicion = "1ra";
            entidad.Fecha_Lanzamiento = DateTime.Now;
            entidad.Editorial = editoriales.Id;
            entidad.Pais = paises.Id;
            entidad.Tipo = tipos.Id;

            // Generar un ISBN corto y seguro (sin guiones). Máx 20 caracteres.
            var guidCompact = Guid.NewGuid().ToString("N").Substring(0, 13);
            entidad.Isbn = $"ISBN-{guidCompact}"; // longitud aprox 18

            return entidad;
        }

        public static Existencias Existencias(Libros libros)
        {
            var entidad = new Existencias();
            entidad.Libro = libros.Id;
            entidad.Ejemplares = 5; // >= 0 por el CHECK en la entidad
            return entidad;
        }

        public static Estados Estados()
        {
            var entidad = new Estados();
            entidad.Nombre_Estado = "Estado Prueba";
            return entidad;
        }

        public static EstadosExistencias EstadosExistencias(Existencias existencias, Estados estados)
        {
            var entidad = new EstadosExistencias();
            entidad.Existencia = existencias.Id;
            entidad.Estado = estados.Id;
            entidad.Fecha_Cambio = DateTime.Now;
            return entidad;
        }

        public static Usuarios Usuarios()
        {
            var entidad = new Usuarios();
            entidad.Nombre = "Usuario Prueba";
            entidad.Documento = Guid.NewGuid().ToString("N").Substring(0, 10);
            entidad.Direccion = "Calle Prueba";
            entidad.Telefono = "3001234567";
            entidad.Correo = $"usuario_{Guid.NewGuid().ToString("N").Substring(0, 8)}@prueba.com";
            entidad.Contraseña = "123456"; // en producción deberías guardar un hash
            entidad.Fecha_Nacimiento = DateTime.Now.AddYears(-25);
            return entidad;
        }

        public static Prestamos Prestamos(Usuarios usuarios, Existencias existencias, TiposPrestamos tiposPrestamos)
        {
            var entidad = new Prestamos();
            entidad.Usuario = usuarios.Id;
            entidad.Existencia = existencias.Id;
            entidad.Tipo_Prestamo = tiposPrestamos.Id;
            entidad.Fecha_Prestamo = DateTime.Now;
            entidad.Fecha_Devolucion = DateTime.Now.AddDays(7);
            entidad.Fecha_Entrega_Real = null;
            return entidad;
        }

        public static Sanciones Sanciones(Usuarios usuarios)
        {
            var entidad = new Sanciones();
            entidad.Usuario = usuarios.Id;
            entidad.Descripcion = "Sanción de prueba";
            entidad.Fecha_Inicio = DateTime.Now;
            entidad.Fecha_Fin = DateTime.Now.AddDays(5);
            return entidad;
        }

        public static LibrosAutores LibrosAutores(Libros libros, Autores autores)
        {
            var entidad = new LibrosAutores();
            entidad.Libro = libros.Id;
            entidad.Autor = autores.Id;
            return entidad;
        }

        public static Temas Temas()
        {
            var entidad = new Temas();
            entidad.Nombre_Tema = "Tema Prueba";
            entidad.Area_Conocimiento = "Área Prueba";
            return entidad;
        }

        public static LibrosTemas LibrosTemas(Libros libros, Temas temas)
        {
            var entidad = new LibrosTemas();
            entidad.Libro = libros.Id;
            entidad.Tema = temas.Id;
            return entidad;
        }

        public static TiposPrestamos TiposPrestamos()
        {
            var entidad = new TiposPrestamos();
            entidad.Descripcion = "Préstamo Normal";
            return entidad;
        }
    }
}
