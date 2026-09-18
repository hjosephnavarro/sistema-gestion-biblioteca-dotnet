(function() {
    'use strict';

    // CONFIGURACIÓN
    const CONFIG = {
        viewKey: 'bibliotechView',
        searchDelay: 300
    };

    // ESTADO
    const state = {
        view: localStorage.getItem(CONFIG.viewKey) || 'table',
        searchTerm: '',
        prestamosActivos: [],
        sanciones: [],
        isPrestamosVisible: false,
        isSancionesVisible: false
    };

    // DOM REFERENCIAS
    const DOM = {
        search: document.getElementById('searchInput'),
        clearBtn: document.getElementById('clearSearch'),
        searchStats: document.getElementById('searchStats'),
        tableView: document.getElementById('tableView'),
        gridView: document.getElementById('gridView'),
        tableViewBtn: document.getElementById('tableViewBtn'),
        gridViewBtn: document.getElementById('gridViewBtn'),
        prestamosSection: document.getElementById('prestamosActivosSection'),
        prestamosBody: document.getElementById('prestamosActivosBody'),
        prestamosCount: document.getElementById('prestamosActivosCount'),
        prestamosCard: document.getElementById('prestamosActivosCard'),
        verPrestamosBtn: document.getElementById('verMisPrestamosBtn'),
        cerrarPrestamosBtn: document.getElementById('cerrarPrestamosBtn'),
        sancionesSection: document.getElementById('sancionesSection'),
        sancionesBody: document.getElementById('sancionesBody'),
        verSancionesBtn: document.getElementById('verSancionesBtn'),
        cerrarSancionesBtn: document.getElementById('cerrarSancionesBtn'),
        modal: document.getElementById('modalDetallePrestamo'),
        modalBody: document.getElementById('modalDetalleBody'),
        toast: document.getElementById('toast'),
        availableCount: document.getElementById('availableBooksCount')
    };

    // UTILIDADES
    function getAntiForgeryToken() {
        const input = document.querySelector('input[name="__RequestVerificationToken"]');
        return input ? input.value : '';
    }

    function showToast(message, isError = false) {
        const toast = DOM.toast;
        if (!toast) { 
            console.log(message);
            return; 
        }
        toast.textContent = message;
        toast.className = 'toast show' + (isError ? ' error' : '');
        clearTimeout(toast._timeout);
        toast._timeout = setTimeout(() => toast.className = 'toast', 3500);
    }

    function formatDate(dateStr) {
        if (!dateStr) return 'N/A';
        try {
            const date = new Date(dateStr);
            return date.toLocaleDateString('es-ES', { 
                day: '2-digit', 
                month: '2-digit', 
                year: 'numeric' 
            });
        } catch {
            return dateStr;
        }
    }

    // FILTRO DE LIBROS
    function filterBooks() {
        const term = DOM.search?.value.toLowerCase().trim() || '';
        state.searchTerm = term;

        const rows = document.querySelectorAll('#booksTable tbody tr');
        const cards = document.querySelectorAll('.book-card');
        let matchCount = 0;

        rows.forEach(row => {
            const title = row.dataset.title || '';
            const isbn = row.dataset.isbn || '';
            const matches = term === '' || title.includes(term) || isbn.includes(term);
            row.style.display = matches ? '' : 'none';
            if (matches) matchCount++;
        });

        cards.forEach(card => {
            const title = card.dataset.title || '';
            const isbn = card.dataset.isbn || '';
            const matches = term === '' || title.includes(term) || isbn.includes(term);
            card.style.display = matches ? '' : 'none';
        });

        if (DOM.clearBtn) {
            DOM.clearBtn.style.display = term ? 'flex' : 'none';
        }

        if (DOM.searchStats) {
            DOM.searchStats.textContent = term === '' ? '' : `${matchCount} resultado${matchCount !== 1 ? 's' : ''}`;
        }

        updateAvailableCount();
    }

    function clearSearch() {
        if (DOM.search) {
            DOM.search.value = '';
            filterBooks();
            DOM.search.focus();
        }
    }

    function updateAvailableCount() {
        const visibleRows = document.querySelectorAll('#booksTable tbody tr:not([style*="display: none"])');
        const available = Array.from(visibleRows).filter(row => parseInt(row.dataset.available || '0') > 0).length;
        if (DOM.availableCount) {
            DOM.availableCount.textContent = available;
        }
    }

    // VISTA (Tabla / Grid)
    function setView(view) {
        state.view = view;
        localStorage.setItem(CONFIG.viewKey, view);

        if (view === 'table') {
            DOM.tableView.style.display = 'block';
            DOM.gridView.style.display = 'none';
            DOM.tableViewBtn.classList.add('active');
            DOM.gridViewBtn.classList.remove('active');
        } else {
            DOM.tableView.style.display = 'none';
            DOM.gridView.style.display = 'block';
            DOM.tableViewBtn.classList.remove('active');
            DOM.gridViewBtn.classList.add('active');
        }
        filterBooks();
    }

    // PRÉSTAMOS - CARGAR
    async function cargarPrestamosActivos() {
        try {
            const token = getAntiForgeryToken();
            const resp = await fetch('?handler=PrestamosActivosAjax', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': token
                }
            });
            if (!resp.ok) throw new Error('Error al cargar préstamos');
            const data = await resp.json();
            state.prestamosActivos = data.prestamos || [];
            if (DOM.prestamosCount) {
                DOM.prestamosCount.textContent = state.prestamosActivos.length;
            }
            return state.prestamosActivos;
        } catch (err) {
            console.error('Error cargando préstamos:', err);
            return [];
        }
    }

    // PRÉSTAMOS - RENDERIZAR
    function renderizarPrestamosActivos() {
        if (!DOM.prestamosBody) return;

        if (state.prestamosActivos.length === 0) {
            DOM.prestamosBody.innerHTML = '<tr><td colspan="5" class="text-center">📭 No tienes préstamos activos</td></tr>';
            return;
        }

        const hoy = new Date();
        DOM.prestamosBody.innerHTML = state.prestamosActivos.map(p => {
            const fechaDev = new Date(p.fechaDevolucion);
            const vencido = fechaDev < hoy;
            const estadoBadge = vencido 
                ? '<span class="badge vencido"><i class="bi bi-exclamation-triangle-fill"></i> Vencido</span>'
                : '<span class="badge prestado"><i class="bi bi-clock-fill"></i> Activo</span>';
            return `
                <tr data-prestamoid="${p.id}">
                    <td><strong>${escapeHtml(p.libroTitulo)}</strong><br><small>ISBN: ${p.isbn || 'N/A'}</small></td>
                    <td>${formatDate(p.fechaPrestamo)}</td>
                    <td>${formatDate(p.fechaDevolucion)}</td>
                    <td>${estadoBadge}</td>
                    <td>
                        <button class="btn-detalle" data-prestamoid="${p.id}">
                            <i class="bi bi-eye"></i> Detalle
                        </button>
                    </td>
                </tr>
            `;
        }).join('');

        document.querySelectorAll('.btn-detalle').forEach(btn => {
            btn.addEventListener('click', () => mostrarDetallePrestamo(btn.dataset.prestamoid));
        });
    }

    // PRÉSTAMOS - DETALLE
    function mostrarDetallePrestamo(prestamoId) {
        const prestamo = state.prestamosActivos.find(p => p.id == prestamoId);
        if (!prestamo) return;

        if (DOM.modalBody) {
            DOM.modalBody.innerHTML = `
                <div class="detalle-prestamo">
                    <p><strong>Libro:</strong> ${escapeHtml(prestamo.libroTitulo)}</p>
                    <p><strong>ISBN:</strong> ${prestamo.isbn || 'N/A'}</p>
                    <p><strong>Fecha Préstamo:</strong> ${formatDate(prestamo.fechaPrestamo)}</p>
                    <p><strong>Fecha Devolución:</strong> ${formatDate(prestamo.fechaDevolucion)}</p>
                    <p><strong>Tipo Préstamo:</strong> ${prestamo.tipoPrestamo || 'Estándar'}</p>
                    <hr>
                    <p><strong>Instrucciones:</strong> Por favor devuelve el libro en la fecha indicada.</p>
                </div>
            `;
        }
        if (DOM.modal) DOM.modal.style.display = 'flex';
    }

    // PRÉSTAMOS - MOSTRAR/OCULTAR SECCIÓN
    async function mostrarSeccionPrestamos() {
        await cargarPrestamosActivos();
        renderizarPrestamosActivos();
        if (DOM.prestamosSection) {
            DOM.prestamosSection.style.display = 'block';
            DOM.prestamosSection.scrollIntoView({ behavior: 'smooth' });
            state.isPrestamosVisible = true;
        }
        // Ocultar sanciones si están visibles
        if (state.isSancionesVisible) {
            ocultarSeccionSanciones();
        }
    }

    function ocultarSeccionPrestamos() {
        if (DOM.prestamosSection) {
            DOM.prestamosSection.style.display = 'none';
            state.isPrestamosVisible = false;
        }
    }

    // SANCIONES - CARGAR
    async function cargarSanciones() {
        try {
            const token = getAntiForgeryToken();
            const resp = await fetch('?handler=SancionesAjax', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': token
                }
            });
            if (!resp.ok) throw new Error('Error al cargar sanciones');
            const data = await resp.json();
            state.sanciones = data.sanciones || [];
            return state.sanciones;
        } catch (err) {
            console.error('Error cargando sanciones:', err);
            return [];
        }
    }

    // SANCIONES - RENDERIZAR
    function renderizarSanciones() {
        if (!DOM.sancionesBody) return;

        if (state.sanciones.length === 0) {
            DOM.sancionesBody.innerHTML = '<tr><td colspan="4" class="text-center">✅ No tienes sanciones activas</td></tr>';
            return;
        }

        const hoy = new Date();
        DOM.sancionesBody.innerHTML = state.sanciones.map(s => {
            const fechaFin = s.fechaFin ? new Date(s.fechaFin) : null;
            const activa = !fechaFin || fechaFin > hoy;
            const estadoBadge = activa 
                ? '<span class="badge sancion-activa"><i class="bi bi-exclamation-circle-fill"></i> Activa</span>'
                : '<span class="badge sancion-finalizada"><i class="bi bi-check-circle-fill"></i> Finalizada</span>';
            return `
                <tr>
                    <td>${escapeHtml(s.descripcion)}</td>
                    <td>${formatDate(s.fechaInicio)}</td>
                    <td>${s.fechaFin ? formatDate(s.fechaFin) : 'Indefinida'}</td>
                    <td>${estadoBadge}</td>
                </tr>
            `;
        }).join('');
    }

    // SANCIONES - MOSTRAR/OCULTAR SECCIÓN
    async function mostrarSeccionSanciones() {
        await cargarSanciones();
        renderizarSanciones();
        if (DOM.sancionesSection) {
            DOM.sancionesSection.style.display = 'block';
            DOM.sancionesSection.scrollIntoView({ behavior: 'smooth' });
            state.isSancionesVisible = true;
        }
        // Ocultar préstamos si están visibles
        if (state.isPrestamosVisible) {
            ocultarSeccionPrestamos();
        }
    }

    function ocultarSeccionSanciones() {
        if (DOM.sancionesSection) {
            DOM.sancionesSection.style.display = 'none';
            state.isSancionesVisible = false;
        }
    }

    // PRÉSTAMOS - SOLICITAR
    async function solicitarPrestamo(libroId, button, titulo) {
        const token = getAntiForgeryToken();
        const originalText = button.innerHTML;

        try {
            button.disabled = true;
            button.innerHTML = '<i class="bi bi-hourglass-split"></i> Procesando...';

            const resp = await fetch('?handler=SolicitarPrestamoAjax', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': token
                },
                body: JSON.stringify({ libroId: libroId })
            });

            if (!resp.ok) throw new Error('HTTP ' + resp.status);
            const data = await resp.json();

            if (data.success) {
                showToast(`✅ ¡Solicitud exitosa! El libro "${titulo}" ha sido reservado.`, false);
                await actualizarDisponibilidad(libroId, data.available);
                await cargarPrestamosActivos();
                if (state.isPrestamosVisible) renderizarPrestamosActivos();
                if (DOM.prestamosCount) {
                    DOM.prestamosCount.textContent = state.prestamosActivos.length;
                }
                // Actualizar sanciones también
                await cargarSanciones();
                if (state.isSancionesVisible) renderizarSanciones();
            } else {
                showToast(data.message || '❌ No se pudo completar la solicitud', true);
                button.disabled = false;
                button.innerHTML = originalText;
            }
        } catch (err) {
            console.error(err);
            showToast('❌ Error de conexión. Intenta nuevamente.', true);
            button.disabled = false;
            button.innerHTML = originalText;
        }
    }

    async function actualizarDisponibilidad(libroId, available) {
        document.querySelectorAll(`tr[data-libroid="${libroId}"]`).forEach(row => {
            row.dataset.available = available;
            const badge = row.querySelector('.badge');
            const totalSpan = row.querySelector('.total-copies');
            
            if (available > 0) {
                if (badge) {
                    badge.className = 'badge available';
                    badge.innerHTML = `<i class="bi bi-check-circle-fill"></i> ${available} disponible(s)`;
                }
            } else {
                if (badge) {
                    badge.className = 'badge out';
                    badge.innerHTML = '<i class="bi bi-x-circle-fill"></i> Sin stock';
                }
            }
            
            const btn = row.querySelector('.btn-prestar');
            if (btn) btn.disabled = available <= 0;
        });

        document.querySelectorAll(`.book-card[data-libroid="${libroId}"]`).forEach(card => {
            card.dataset.available = available;
            const badge = card.querySelector('.badge');
            if (available > 0) {
                if (badge) {
                    badge.className = 'badge available';
                    badge.innerHTML = `<i class="bi bi-check-circle-fill"></i> ${available} disponibles`;
                }
            } else {
                if (badge) {
                    badge.className = 'badge out';
                    badge.innerHTML = '<i class="bi bi-x-circle-fill"></i> Sin stock';
                }
            }
            const btn = card.querySelector('.btn-prestar-card');
            if (btn) btn.disabled = available <= 0;
        });

        updateAvailableCount();
    }

    // UTILIDAD - ESCAPE HTML
    function escapeHtml(text) {
        if (!text) return '';
        const div = document.createElement('div');
        div.textContent = text;
        return div.innerHTML;
    }

    // EVENTOS
    function setupEventListeners() {
        // Búsqueda
        if (DOM.search) {
            DOM.search.addEventListener('input', filterBooks);
            DOM.search.addEventListener('keypress', (e) => {
                if (e.key === 'Enter') filterBooks();
            });
        }
        if (DOM.clearBtn) {
            DOM.clearBtn.addEventListener('click', clearSearch);
        }

        // Vistas
        if (DOM.tableViewBtn) {
            DOM.tableViewBtn.addEventListener('click', () => setView('table'));
        }
        if (DOM.gridViewBtn) {
            DOM.gridViewBtn.addEventListener('click', () => setView('grid'));
        }

        // Préstamos
        if (DOM.prestamosCard) {
            DOM.prestamosCard.addEventListener('click', mostrarSeccionPrestamos);
        }
        if (DOM.verPrestamosBtn) {
            DOM.verPrestamosBtn.addEventListener('click', mostrarSeccionPrestamos);
        }
        if (DOM.cerrarPrestamosBtn) {
            DOM.cerrarPrestamosBtn.addEventListener('click', ocultarSeccionPrestamos);
        }

        // Sanciones
        if (DOM.verSancionesBtn) {
            DOM.verSancionesBtn.addEventListener('click', mostrarSeccionSanciones);
        }
        if (DOM.cerrarSancionesBtn) {
            DOM.cerrarSancionesBtn.addEventListener('click', ocultarSeccionSanciones);
        }

        // Modal - cerrar al hacer clic fuera
        window.onclick = function(event) {
            if (event.target === DOM.modal) {
                DOM.modal.style.display = 'none';
            }
        };

        // Cerrar modal con Escape
        document.addEventListener('keydown', function(e) {
            if (e.key === 'Escape' && DOM.modal) {
                DOM.modal.style.display = 'none';
            }
        });

        // Botones de préstamo (delegación de eventos)
        document.addEventListener('click', function(e) {
            const btn = e.target.closest('.btn-prestar, .btn-prestar-card');
            if (btn && !btn.disabled) {
                e.preventDefault();
                const libId = parseInt(btn.dataset.libroid);
                const titulo = btn.dataset.titulo || 'este libro';
                if (!libId) return;
                if (confirm(`¿Deseas solicitar el libro "${titulo}"?`)) {
                    solicitarPrestamo(libId, btn, titulo);
                }
            }
        });
    }

    // INICIALIZACIÓN
    async function init() {
        console.log('📚 Inicializando ClienteApp...');

        // Restaurar vista guardada
        const savedView = localStorage.getItem(CONFIG.viewKey);
        setView(savedView === 'grid' ? 'grid' : 'table');

        // Cargar préstamos y sanciones
        await cargarPrestamosActivos();
        await cargarSanciones();

        // Configurar eventos
        setupEventListeners();

        // Actualizar contador de disponibles
        updateAvailableCount();

        console.log('✅ ClienteApp inicializado correctamente');
        console.log(`📊 Préstamos activos: ${state.prestamosActivos.length}`);
        console.log(`⚠️ Sanciones: ${state.sanciones.length}`);
    }

    // Iniciar cuando el DOM esté listo
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }

    // EXPORTAR FUNCIONES PARA USO EN CONSOLA (DEBUG)
    window.ClienteApp = {
        recargar: init,
        prestamos: () => state.prestamosActivos,
        sanciones: () => state.sanciones,
        mostrarPrestamos: mostrarSeccionPrestamos,
        mostrarSanciones: mostrarSeccionSanciones
    };

})();