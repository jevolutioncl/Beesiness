document.addEventListener('DOMContentLoaded', function () {
    var tipoColmenaSelect = document.getElementById('tipoColmenaSelect');
    var ubicacionSelect = document.getElementById('UbicacionPredeterminada'); 

    tipoColmenaSelect.addEventListener('focus', function () {
        var defaultOption = document.getElementById('defaultOption');
        if (defaultOption) {
            tipoColmenaSelect.removeChild(defaultOption);
        }
    });

    ubicacionSelect.addEventListener('focus', function () { 
        var defaultUbicacionOption = document.getElementById('defaultUbicacionOption');
        if (defaultUbicacionOption) {
            ubicacionSelect.removeChild(defaultUbicacionOption);
        }
    });
    ubicacionSelect.addEventListener('change', function (e) {
        if (e.target.value) {
            var selectedLocation = JSON.parse(e.target.value);
            console.log("Ubicación seleccionada:", selectedLocation); 
            updateMapToLocation(selectedLocation);
        }
    });
});

document.addEventListener('DOMContentLoaded', function () {
    var numIdentificadorInput = document.getElementById('numIdentificador');
    var submitButton = document.querySelector('input[type="submit"]');
    var validationMessage = document.getElementById('validationMessage'); 

    numIdentificadorInput.addEventListener('change', function () {
        var numIdentificador = numIdentificadorInput.value;

        if (numIdentificador) {
            fetch(`/api/colmenas/validar-identificador/${numIdentificador}`)
                .then(response => response.json())
                .then(data => {
                    if (data.ocupado) {
                        validationMessage.textContent = 'Este número de identificación ya está ocupado, intente con otro o revise las colmenas ya creadas.';
                        validationMessage.style.display = 'block'; // 
                        submitButton.disabled = true;
                    } else {
                        validationMessage.style.display = 'none';
                        submitButton.disabled = false;
                    }
                })
                .catch(error => {
                    console.error('Error al validar el identificador:', error);
                });
        }
    });
});
document.addEventListener('DOMContentLoaded', function () {
    var fechaIngreso = document.getElementById('fechaIngreso');
    var fechaActual = new Date().toISOString().substring(0, 10); // Formato AAAA-MM-DD
    fechaIngreso.value = fechaActual;
});