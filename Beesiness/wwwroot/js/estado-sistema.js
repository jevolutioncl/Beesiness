function actualizarEstadoUI(componente, estaConectado) {
    const textoEstado = document.getElementById(`textoEstado${componente}`);
    if (textoEstado) {
        textoEstado.textContent = estaConectado ? 'Conectado' : 'Desconectado';
        textoEstado.classList.remove('estado-conectado', 'estado-desconectado');
        textoEstado.classList.add(estaConectado ? 'estado-conectado' : 'estado-desconectado');
        console.log(`Actualización de ${componente}: ${estaConectado ? 'Conectado' : 'Desconectado'}`);
    } else {
        console.log(`El elemento con ID textoEstado${componente} no se encuentra en el DOM.`);
    }
}

function verificarEstadoSistema() {
    console.log("Verificando el estado del sistema...");
    fetch('/api/Health/GetSystemHealth')
        .then(response => response.json())
        .then(data => {
            console.log("Datos recibidos:", data);
            actualizarEstadoUI('Frontend', data.frontend);
            actualizarEstadoUI('Backend', data.backend);
            actualizarEstadoUI('Database', data.database);
            actualizarEstadoUI('Arduino', data.arduino);
        })
        .catch(error => console.error('Error al verificar el estado del sistema:', error));
}

document.getElementById('verificarEstadoSistema').addEventListener('click', verificarEstadoSistema);

document.addEventListener('DOMContentLoaded', verificarEstadoSistema);

