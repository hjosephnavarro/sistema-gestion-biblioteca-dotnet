using System;
using System.Collections.Generic;
using Dominio.Entidades;
using Repositorio.Implementaciones;
using Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class PrestamosPrueba
    {
        private readonly IConexion? iConexion;

        private List<Prestamos>? lista;
        private Prestamos? entidad;

        private Usuarios? usuario;
        private Existencias? existencia;
        private TiposPrestamos? tipoPrestamo;

        private Editoriales? editorial;
        private Paises? pais;
        private Tipos? tipoLibro;
        private Libros? libro;
        private Estados? estado;
        private EstadosExistencias? estadosExistencia;

        public PrestamosPrueba()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.IsTrue(Guardar());
            Assert.IsTrue(Modificar());
            Assert.IsTrue(Listar());
            Assert.IsTrue(Borrar());
        }

        public bool Guardar()
        {
            // Crear dependencias principales
            this.usuario = EntidadesNucleo.Usuarios()!;
            this.tipoPrestamo = EntidadesNucleo.TiposPrestamos()!;
            this.estado = EntidadesNucleo.Estados()!;

            iConexion!.Usuarios!.Add(this.usuario);
            iConexion.TiposPrestamos!.Add(this.tipoPrestamo);
            iConexion.Estados!.Add(this.estado);
            iConexion.SaveChanges();

            // Crear dependencias del libro
            this.editorial = EntidadesNucleo.Editoriales()!;
            this.pais = EntidadesNucleo.Paises()!;
            this.tipoLibro = EntidadesNucleo.Tipos()!;

            iConexion.Editoriales!.Add(this.editorial);
            iConexion.Paises!.Add(this.pais);
            iConexion.Tipos!.Add(this.tipoLibro);
            iConexion.SaveChanges();

            // Crear libro
            this.libro = EntidadesNucleo.Libros(this.editorial, this.pais, this.tipoLibro)!;
            iConexion.Libros!.Add(this.libro);
            iConexion.SaveChanges();

            // Crear existencia
            this.existencia = EntidadesNucleo.Existencias(this.libro)!;
            iConexion.Existencias!.Add(this.existencia);
            iConexion.SaveChanges();

            // Crear estado de existencia
            this.estadosExistencia = EntidadesNucleo.EstadosExistencias(this.existencia, this.estado)!;
            iConexion.EstadosExistencias!.Add(this.estadosExistencia);
            iConexion.SaveChanges();

            // Crear préstamo
            this.entidad = EntidadesNucleo.Prestamos(this.usuario, this.existencia, this.tipoPrestamo)!;
            // 🔧 Asegurar fechas correctas
            this.entidad.Fecha_Prestamo = DateTime.Now;
            this.entidad.Fecha_Devolucion = DateTime.Now.AddDays(7);
            this.entidad.Fecha_Entrega_Real = null;

            iConexion.Prestamos!.Add(this.entidad);
            iConexion.SaveChanges();

            return true;
        }

        public bool Modificar()
        {
            if (this.entidad == null) return false;

            this.entidad.Fecha_Devolucion = DateTime.Now.AddDays(10);
            var entry = iConexion!.Entry<Prestamos>(this.entidad);
            entry.State = EntityState.Modified;
            iConexion.SaveChanges();
            return true;
        }

        public bool Listar()
        {
            this.lista = iConexion!.Prestamos!.ToList();
            return lista != null && lista.Count > 0;
        }

        public bool Borrar()
        {
            if (this.entidad != null)
            {
                iConexion!.Prestamos!.Remove(this.entidad);
                iConexion.SaveChanges();
            }

            if (this.estadosExistencia != null)
            {
                iConexion!.EstadosExistencias!.Remove(this.estadosExistencia);
                iConexion.SaveChanges();
            }

            if (this.existencia != null)
            {
                iConexion!.Existencias!.Remove(this.existencia);
                iConexion.SaveChanges();
            }

            if (this.libro != null)
            {
                iConexion!.Libros!.Remove(this.libro);
                iConexion.SaveChanges();
            }

            if (this.estado != null)
            {
                iConexion!.Estados!.Remove(this.estado);
                iConexion.SaveChanges();
            }

            if (this.editorial != null)
            {
                iConexion!.Editoriales!.Remove(this.editorial);
                iConexion.SaveChanges();
            }

            if (this.pais != null)
            {
                iConexion!.Paises!.Remove(this.pais);
                iConexion.SaveChanges();
            }

            if (this.tipoLibro != null)
            {
                iConexion!.Tipos!.Remove(this.tipoLibro);
                iConexion.SaveChanges();
            }

            if (this.usuario != null)
            {
                iConexion!.Usuarios!.Remove(this.usuario);
                iConexion.SaveChanges();
            }

            if (this.tipoPrestamo != null)
            {
                iConexion!.TiposPrestamos!.Remove(this.tipoPrestamo);
                iConexion.SaveChanges();
            }

            return true;
        }
    }
}
