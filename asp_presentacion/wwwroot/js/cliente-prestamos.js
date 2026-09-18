(function() {
    'use strict';

    // Exportar funciones al objeto global
    window.PrestamosModule = {
        cargar: cargarPrestamosActivos,
        renderizar: renderizarPrestamosActivos,
        solicitar: solicitarPrestamo,
        mostrarSeccion: mostrarSeccionPrestamos,
        ocultarSeccion: ocultarSeccionPrestamos
    };

    // ... (el código de préstamos se mueve aquí)

})();