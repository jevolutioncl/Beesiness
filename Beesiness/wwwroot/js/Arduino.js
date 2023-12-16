function actualizarEstadoUI(estaConectado) {
    const spanEstado = document.getElementById('arduinoConectado');
    const indicador = document.getElementById('indicadorConexion');
    spanEstado.textContent = estaConectado ? 'true' : 'false';
    spanEstado.className = estaConectado ? 'conectado' : 'desconectado';
    indicador.className = `indicador-conexion ${estaConectado ? 'conectado' : 'desconectado'}`;
}

function verificarEstadoArduino() {
    fetch(verificarEstadoUrl)
        .then(response => response.json())
        .then(data => actualizarEstadoUI(data.estaConectado))
        .catch(error => console.error('Error al verificar el estado del Arduino:', error));
}

setInterval(verificarEstadoArduino, 1800000); // 1800000ms = 30min
document.addEventListener('DOMContentLoaded', verificarEstadoArduino);
