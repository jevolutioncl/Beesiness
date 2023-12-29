var map;
var userPin;

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

function loadMapScenario() {
    map = new Microsoft.Maps.Map('#myMap', {
        center: new Microsoft.Maps.Location(-35.31917337366901, -71.36038685586249),
        zoom: 250,
        mapTypeId: Microsoft.Maps.MapTypeId.aerial
    });

    var currentLat = document.getElementById('Latitude').value;
    var currentLong = document.getElementById('Longitude').value;
    if (currentLat && currentLong) {
        var currentLocation = new Microsoft.Maps.Location(currentLat, currentLong);
        addUserPin(map, currentLocation);
        map.setView({ center: currentLocation, zoom: 250 });
    }

    Microsoft.Maps.Events.addHandler(map, 'click', function (e) {
        var point = new Microsoft.Maps.Point(e.getX(), e.getY());
        var location = e.target.tryPixelToLocation(point);

        document.getElementById('Latitude').value = location.latitude.toFixed(6);
        document.getElementById('Longitude').value = location.longitude.toFixed(6);

        addUserPin(map, location); 
    });

}

function addUserPin(map, location) {
    if (userPin) {
        map.entities.remove(userPin);
    }
    userPin = new Microsoft.Maps.Pushpin(location, { draggable: true });
    map.entities.push(userPin);

    Microsoft.Maps.Events.addHandler(userPin, 'dragend', function (e) {
        var location = userPin.getLocation();
        document.getElementById('Latitude').value = location.latitude.toFixed(6);
        document.getElementById('Longitude').value = location.longitude.toFixed(6);
        reverseGeocode(location);
    });
}

function reverseGeocode(location) {
    var geocodeRequest = "https://dev.virtualearth.net/REST/v1/Locations/" + location.latitude + "," + location.longitude + "?o=json&key=AmrnF-1-dvaNYDUkUuogYW8SOvy2cut1Xdxs4EzsahlVv8HrT_MvGx0HPKP8dOlZ";
    fetch(geocodeRequest)
        .then(response => response.json())
        .then(data => {
            if (data.resourceSets.length > 0 && data.resourceSets[0].resources.length > 0) {
                var locationName = data.resourceSets[0].resources[0].name;
                document.getElementById('locationName').innerText = "Ubicación escogida: " + locationName;
                document.getElementById('UbicacionColmena').value = locationName;
            } else {
                document.getElementById('locationName').innerText = "Nombre de ubicación no encontrado";
            }
        });
}

