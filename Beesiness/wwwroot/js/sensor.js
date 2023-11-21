$(document).ready(function () {
    function actualizarTemperatura() {
        $.ajax({
            url: '/SensorData/UltimaTemperatura/3002',
            type: 'GET',
            cache: false, // Desactiva el caché para esta solicitud
            success: function (data) {
                $('#temperatura').text(data + ' °C');
            },
            error: function (xhr, status, error) {
                console.error("Error al obtener la temperatura:", xhr.responseText);
                $('#temperatura').text('Error al obtener la temperatura');
            }
        });
    }

    setInterval(actualizarTemperatura, 1000);
    actualizarTemperatura();
});