// admin-app.js - JavaScript para el panel de administración

// ============================================
// TABS
// ============================================
document.querySelectorAll('.tab-btn').forEach(function(btn) {
    btn.addEventListener('click', function() {
        var tab = this.dataset.tab;
        document.querySelectorAll('.tab-btn').forEach(function(b) { b.classList.remove('active'); });
        document.querySelectorAll('.tab-content').forEach(function(c) { c.classList.remove('active'); });
        this.classList.add('active');
        document.getElementById('tab' + tab.charAt(0).toUpperCase() + tab.slice(1)).classList.add('active');
    });
});

// ============================================
// TOAST
// ============================================
function mostrarToast(mensaje, esError) {
    var toast = document.getElementById('toast');
    toast.textContent = mensaje;
    toast.className = 'toast show' + (esError ? ' error' : '');
    clearTimeout(toast._timeout);
    toast._timeout = setTimeout(function() { toast.className = 'toast'; }, 3000);
}

// ============================================
// MODAL
// ============================================
function cerrarModal() {
    document.getElementById('modalEditar').style.display = 'none';
}

// ============================================
// LIBROS
// ============================================
function abrirModalEditarLibro(id, titulo, isbn, edicion, fechaLanzamiento) {
    document.getElementById('modalTitulo').innerHTML = 'Editar Libro';
    document.getElementById('modalBody').innerHTML = `
        <form method="post" asp-page-handler="EditLibro">
            <input type="hidden" name="id" value="${id}" />
            <div class="form-group"><label>Titulo</label><input type="text" name="titulo" class="form-control" value="${titulo.replace(/\\/g, '\\\\')}" required /></div>
            <div class="form-group"><label>ISBN</label><input type="text" name="isbn" class="form-control" value="${isbn.replace(/\\/g, '\\\\')}" /></div>
            <div class="form-group"><label>Edicion</label><input type="text" name="edicion" class="form-control" value="${edicion.replace(/\\/g, '\\\\')}" /></div>
            <div class="form-group"><label>Fecha Lanzamiento</label><input type="date" name="fechaLanzamiento" class="form-control" value="${fechaLanzamiento}" /></div>
            <button type="submit" class="btn-primary" style="width:100%"><i class="bi bi-save"></i> Guardar Cambios</button>
        </form>
    `;
    document.getElementById('modalEditar').style.display = 'flex';
}

// ============================================
// AUTORES
// ============================================
function abrirModalEditarAutor(id, nombre, nacionalidad) {
    document.getElementById('modalTitulo').innerHTML = 'Editar Autor';
    document.getElementById('modalBody').innerHTML = `
        <form method="post" asp-page-handler="EditAutor">
            <input type="hidden" name="id" value="${id}" />
            <div class="form-group"><label>Nombre</label><input type="text" name="nombre" class="form-control" value="${nombre.replace(/\\/g, '\\\\')}" required /></div>
            <div class="form-group"><label>Nacionalidad</label><input type="text" name="nacionalidad" class="form-control" value="${nacionalidad.replace(/\\/g, '\\\\')}" /></div>
            <button type="submit" class="btn-primary" style="width:100%"><i class="bi bi-save"></i> Guardar Cambios</button>
        </form>
    `;
    document.getElementById('modalEditar').style.display = 'flex';
}

// ============================================
// EDITORIALES
// ============================================
function abrirModalEditarEditorial(id, nombre, sitioWeb) {
    document.getElementById('modalTitulo').innerHTML = 'Editar Editorial';
    document.getElementById('modalBody').innerHTML = `
        <form method="post" asp-page-handler="EditEditorial">
            <input type="hidden" name="id" value="${id}" />
            <div class="form-group"><label>Nombre</label><input type="text" name="nombre" class="form-control" value="${nombre.replace(/\\/g, '\\\\')}" required /></div>
            <div class="form-group"><label>Sitio Web</label><input type="text" name="sitioWeb" class="form-control" value="${sitioWeb.replace(/\\/g, '\\\\')}" /></div>
            <button type="submit" class="btn-primary" style="width:100%"><i class="bi bi-save"></i> Guardar Cambios</button>
        </form>
    `;
    document.getElementById('modalEditar').style.display = 'flex';
}

// ============================================
// TEMAS
// ============================================
function abrirModalEditarTema(id, nombre, area) {
    document.getElementById('modalTitulo').innerHTML = 'Editar Tema';
    document.getElementById('modalBody').innerHTML = `
        <form method="post" asp-page-handler="EditTema">
            <input type="hidden" name="id" value="${id}" />
            <div class="form-group"><label>Nombre</label><input type="text" name="nombre" class="form-control" value="${nombre.replace(/\\/g, '\\\\')}" required /></div>
            <div class="form-group"><label>Area de Conocimiento</label><input type="text" name="area" class="form-control" value="${area.replace(/\\/g, '\\\\')}" /></div>
            <button type="submit" class="btn-primary" style="width:100%"><i class="bi bi-save"></i> Guardar Cambios</button>
        </form>
    `;
    document.getElementById('modalEditar').style.display = 'flex';
}

// ============================================
// EXISTENCIAS
// ============================================
function abrirModalEditarExistencia(id, ejemplares) {
    document.getElementById('modalTitulo').innerHTML = 'Editar Existencia';
    document.getElementById('modalBody').innerHTML = `
        <form method="post" asp-page-handler="EditExistencia">
            <input type="hidden" name="id" value="${id}" />
            <div class="form-group"><label>Ejemplares</label><input type="number" name="ejemplares" class="form-control" value="${ejemplares}" required /></div>
            <button type="submit" class="btn-primary" style="width:100%"><i class="bi bi-save"></i> Guardar Cambios</button>
        </form>
    `;
    document.getElementById('modalEditar').style.display = 'flex';
}

// ============================================
// PRESTAMOS
// ============================================
function registrarDevolucion(prestamoId) {
    if (confirm('Registrar devolucion de este prestamo?')) {
        var form = document.createElement('form');
        form.method = 'post';
        form.action = '?handler=RegistrarDevolucion';
        var input = document.createElement('input');
        input.type = 'hidden';
        input.name = 'id';
        input.value = prestamoId;
        form.appendChild(input);
        document.body.appendChild(form);
        form.submit();
    }
}