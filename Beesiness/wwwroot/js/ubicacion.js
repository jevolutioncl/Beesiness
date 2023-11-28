var userPin;
var map;
function mapLoaded() {
    loadMapScenario();

    var ubicacionSelect = document.getElementById('UbicacionPredeterminada');
    if (ubicacionSelect) {
        ubicacionSelect.addEventListener('change', function (e) {
            var selectedLocation = JSON.parse(e.target.value);
            updateMapToLocation(selectedLocation);
        });
    }
}

$(document).ready(function () {
    var colmenaLocation = localStorage.getItem('ColmenaLocation');
    if (colmenaLocation) {
        colmenaLocation = JSON.parse(colmenaLocation);
        $('#Latitude').val(colmenaLocation.latitude);
        $('#Longitude').val(colmenaLocation.longitude);
    }

    var ubicacionSelect = document.getElementById('UbicacionPredeterminada');
    ubicacionSelect.addEventListener('change', function (e) {
        var selectedLocation = JSON.parse(e.target.value);
        updateMapToLocation(selectedLocation);
    });
});
function loadMapScenario() {
    map = new Microsoft.Maps.Map('#myMap', {
        center: new Microsoft.Maps.Location(-35.31917337366901, -71.36038685586249),
        zoom: 250,
        mapTypeId: Microsoft.Maps.MapTypeId.aerial
    });

   
    fetch('/Colmena/ObtenerDatosColmenas')
        .then(response => response.json())
        .then(data => {
            displayColmenas(map, data); 
        })
        .catch(error => {
            console.error('Error al obtener los datos de la colmena: ', error);
        });


    var colmenaLocation = localStorage.getItem('ColmenaLocation');
    if (colmenaLocation) {
        colmenaLocation = JSON.parse(colmenaLocation);
        var savedLoc = new Microsoft.Maps.Location(colmenaLocation.latitude, colmenaLocation.longitude);
        addUserPin(map, savedLoc);
        map.setView({ center: savedLoc, zoom: 250 });

        localStorage.removeItem('ColmenaLocation');
    }

    Microsoft.Maps.Events.addHandler(map, 'click', function (e) {
        var point = new Microsoft.Maps.Point(e.getX(), e.getY());
        var location = e.target.tryPixelToLocation(point);

        document.getElementById('Latitude').value = location.latitude.toFixed(6);
        document.getElementById('Longitude').value = location.longitude.toFixed(6);

        addUserPin(map, location); 

        reverseGeocode(location);
    });
}
function updateMapToLocation(location) {
    console.log("Actualizando mapa a:", location); // Para depuración
    if (map && location && location.Latitude && location.Longitude) {
        var newCenter = new Microsoft.Maps.Location(location.Latitude, location.Longitude);
        map.setView({ center: newCenter, zoom: location.Zoom ? location.Zoom : 250 });

        if (userPin) {
            userPin.setLocation(newCenter);
        }
    } else {
        console.error("Datos de ubicación no válidos:", location);
    }
}

function displayColmenas(map, colmenas) {
    colmenas.forEach(colmena => {
        var location = new Microsoft.Maps.Location(colmena.latitude, colmena.longitude);
        var pin = new Microsoft.Maps.Pushpin(location, {
            icon: '/css/bee-box.png',
            anchor: new Microsoft.Maps.Point(16, 16)
        });
        map.entities.push(pin);
    });
}
function addUserPin(map, location) {
    if (userPin) {
        map.entities.remove(userPin); 
    }
    userPin = new Microsoft.Maps.Pushpin(location); 
    map.entities.push(userPin); 
}
function reverseGeocode(location) {
    var geocodeRequest = "https://dev.virtualearth.net/REST/v1/Locations/" + location.latitude + "," + location.longitude + "?o=json&key=AmrnF-1-dvaNYDUkUuogYW8SOvy2cut1Xdxs4EzsahlVv8HrT_MvGx0HPKP8dOlZ";
    fetch(geocodeRequest)
        .then(response => response.json())
        .then(data => {
            if (data && data.resourceSets && data.resourceSets.length > 0 &&
                data.resourceSets[0].resources && data.resourceSets[0].resources.length > 0) {

                var locationName = data.resourceSets[0].resources[0].name;
                document.getElementById('locationName').innerText = "Ubicación escogida: " + locationName;
                document.getElementById('UbicacionColmena').value = locationName; 
            } else {
                document.getElementById('locationName').innerText = "Nombre de ubicación no encontrado";
            }
        });
}


